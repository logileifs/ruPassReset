using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RUPassReset.Configuration;
using RUPassReset.Service.DBModels;
using RUPassReset.Service.Repositories;

namespace RUPassReset.Service
{
	public class PasswordService
	{
		#region Private variables
		private readonly PasswordRecoveryDataContext _passCtx;
		private readonly MyschoolDataContext _myschoolCtx;
		private EmailService _emailService;
		private static Random _random;
		#endregion

		#region Constructor
		public PasswordService()
		{
			_passCtx = new PasswordRecoveryDataContext();
			_myschoolCtx = new MyschoolDataContext();
			_emailService = new EmailService();
			_random = new Random((int) DateTime.Now.Ticks);
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Creates a reset password token and sends that token to the user.
		/// </summary>
		public UserDTO CreateResetToken(string ssn, string createdByIP)
		{
			// Check if the user exists
			var user = (from mUser in _myschoolCtx.Users
				where mUser.SSN == ssn
				select mUser).SingleOrDefault();

			if (user == null)
				throw new UserNotFoundException();

			// user exists, find it's alternate email
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
			// proceed by create a record in the database
			var passRecovery = new PasswordRecovery
			{
				Token = this.CreateToken(),
				Username = user.Username,
				TimeStamp = DateTime.Now,
				CreatedByIP = createdByIP
			};
			_passCtx.PasswordRecovery.Add(passRecovery);
			_passCtx.SaveChanges();

			// finally, send the email
			_emailService.sendPasswordResetEmail(fullUser, passRecovery.Token);

			return fullUser;
		}

		/// <summary>
		/// Verfies that the token has not expired and not been used.
		/// </summary>
		/// <returns>True if the token is still active, false otherwise.</returns>
		public PasswordRecovery VerifyToken(string token)
		{
			if (token == null)
				return null;

			var result = (from recovery in _passCtx.PasswordRecovery
				where recovery.Token == token
				select recovery).SingleOrDefault();
			// check if token exists
			if (result == null)
				return null;
			// check if token has expired
			if ((DateTime.Now - result.TimeStamp).TotalHours > RUPassResetConfig.Config.TokenLifeTime)
				return null;
			// check if token has been used
			if (result.UsedByIP != null)
				return null;
			// all checks passed, token is active
			return result;
		}

		/// <summary>
		/// Given a token it will reset the password of the user that is assigned to that token. 
		/// </summary>
		public void ResetPassword(string token, string newPassword, string usedByIP)
		{
			var result = (from recovery in _passCtx.PasswordRecovery
				where recovery.Token == token
				select recovery).SingleOrDefault();

			var test = new ADMethodsAccountManagement();
			var moreTest = test.GetUser(result.Username);
			string stuff = "hello";
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
		#endregion
	}
}