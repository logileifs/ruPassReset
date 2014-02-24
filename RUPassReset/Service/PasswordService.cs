using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RUPassReset.Configuration;
using RUPassReset.Service.DBModels;
using RUPassReset.Service.Exceptions;
using RUPassReset.Service.Repositories;

namespace RUPassReset.Service
{
	public class PasswordService
	{
		#region Private variables
		private readonly PasswordRecoveryDataContext _passCtx;
		private readonly MyschoolDataContext _myschoolCtx;
		private EmailService _emailService;
		private ADMethodsAccountManagement _adHelperAccountManagement;
		private static Random _random;
		#endregion

		#region Constructor
		public PasswordService()
		{
			_passCtx = new PasswordRecoveryDataContext();
			_myschoolCtx = new MyschoolDataContext();
			_emailService = new EmailService();
			_adHelperAccountManagement = new ADMethodsAccountManagement();
			_random = new Random((int) DateTime.Now.Ticks);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Creates a reset password token and sends that token to the user.
		/// </summary>
		/// <exception cref="TooManyTriesException">If there are too many attempts at changing the password for this user</exception>
		/// <param name="email">The email to create the token for</param>
		/// <param name="createdByIP">The IP address that is requesting the reset.</param>
		public UserDTO CreateResetToken(string email, string createdByIP)
		{
			// first, get the user
			var fullUser = GetFullUserByEmail(email);

			var gagTime = DateTime.Now.AddHours(-RUPassResetConfig.Config.MaxAttemptsGagTime);

			// check to see if user is making too many requests within a given time
			var result = from passRecovery in _passCtx.PasswordRecovery
				where (passRecovery.Username == fullUser.Username) && (passRecovery.TimeStamp > gagTime)
				select passRecovery;

			if (result.Count() > RUPassResetConfig.Config.MaxAttempts)
				throw new TooManyTriesException();

			// proceed by create a record in the database
			var newRecovery = new PasswordRecovery
			{
				Token = this.CreateToken(),
				Username = fullUser.Username,
				TimeStamp = DateTime.Now,
				CreatedByIP = createdByIP
			};

			_passCtx.PasswordRecovery.Add(newRecovery);
			_passCtx.SaveChanges();

			// finally, send the email
			//_emailService.SendPasswordResetEmail(fullUser, newRecovery.Token);

			return fullUser;
		}

		/// <summary>
		/// Verfies that the token has not expired and not been used.
		/// </summary>
		/// <param name="token">The token that was created for the reset request</param>
		/// <returns>True if the token is still active, false otherwise.</returns>
		public PasswordRecovery VerifyToken(string token)
		{
			if (token == null)
				return null;

			var checkRecovery = (from recovery in _passCtx.PasswordRecovery
				where recovery.Token == token
				select recovery).SingleOrDefault();

			// check if token exists
			if (checkRecovery == null)
				return null;
			// check if token has expired
			if ((DateTime.Now - checkRecovery.TimeStamp).TotalHours > RUPassResetConfig.Config.TokenLifeTime)
				return null;
			// check if token has been used
			if (checkRecovery.UsedByIP != null)
				return null;

			// check if a newer token exists
			var result = from recovery in _passCtx.PasswordRecovery
				where recovery.Username == checkRecovery.Username && recovery.TimeStamp > checkRecovery.TimeStamp
				select recovery;

			if (result.Any())
				return null;

			// all checks passed, token is active
			return checkRecovery;
		}

		/// <summary>
		/// Resets the user's password with the new password given.
		/// <exception cref="PasswordResetFailedException">If something failed in changing the password.</exception>
		/// </summary>
		public void ResetPassword(PasswordRecovery passRecovery, string newPassword, string usedByIP)
		{
			var errMessage = "";
			if (passRecovery == null)
				throw new IllegalTokenException();

			// set the new password
			try
			{
				_adHelperAccountManagement.SetUserPassword(passRecovery.Username, newPassword, out errMessage);
			}
			catch (Exception ex)
			{
				throw new PasswordResetFailedException();
			}

			// update the row and save changes
			passRecovery.UsedByIP = usedByIP;
			_passCtx.SaveChanges();

			var fullUser = GetFullUserByUsername(passRecovery.Username);

			// and finally, send confirmation email
			_emailService.SendPasswordChangedConfirmation(fullUser);
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Creates a random 64 character long token
		/// </summary>
		/// <returns>Random generated token.</returns>
		private string CreateToken()
		{
			string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var builder = new StringBuilder();
			for (int i = 0; i < RUPassResetConfig.Config.TokenSize; i++)
			{
				var ch = allowedChars[Convert.ToInt32(Math.Floor(allowedChars.Length *_random.NextDouble()))];
				builder.Append(ch);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Given an email, it will look it up in the Folk table. If it exists and it's unique, it will try to 
		/// find the user in myscool. 
		/// </summary>
		/// <param name="email">Email to search by</param>
		/// <exception cref="UserNotFoundException">If the user isn't found in myschool</exception>
		/// <exception cref="EmailNotFoundException">If the email wasn't unique or was not found in the Folk table</exception>
		/// <returns>UserDTO</returns>
		private UserDTO GetFullUserByEmail(string email)
		{
			// find the email in Folk table
			var checkResult = from mPerson in _myschoolCtx.Persons
				where mPerson.Email == email
				select mPerson;

			// check if email was found OR if email is linked to multiple accounts
			// If either, just throw EmailNotFoundException because we don't want to say too much 
			if (!checkResult.Any() || checkResult.Count() != 1)
				throw new EmailNotFoundException();

			var person = checkResult.SingleOrDefault();
			
			// check if the user is in myschool
			var user = (from mUser in _myschoolCtx.Users
						where mUser.SSN == person.SSN
						select mUser).SingleOrDefault();

			if (user == null)
				throw new UserNotFoundException();

			var fullUser = new UserDTO
			{
				Name = person.Name,
				Username = user.Username,
				Email = user.Email,
				SecondaryEmail = person.Email
			};

			return fullUser;
		}

		/// <summary>
		/// Finds a user given an HR login username
		/// </summary>
		/// <param name="username">The username to search by</param>
		/// <exception cref="UserNotFoundException">If the username is not found in Users table in myschool database</exception>
		/// <returns>UserDTO</returns>
		private UserDTO GetFullUserByUsername(string username)
		{
			// find the user
			var user = (from mUser in _myschoolCtx.Users
						where mUser.Username == username
						select mUser).SingleOrDefault();

			if (user == null)
				throw new UserNotFoundException();

			// user exists, find it's persona
			var person = (from mPerson in _myschoolCtx.Persons
						  where mPerson.SSN == user.SSN
						  select mPerson).SingleOrDefault();

			var fullUser = new UserDTO
			{
				Name = person.Name,
				Username = user.Username,
				Email = user.Email,
				SecondaryEmail = person.Email
			};

			return fullUser;
		}
		#endregion
	}
}