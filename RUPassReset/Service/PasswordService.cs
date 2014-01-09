using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
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
		/// <param name="ssn"></param>
		/// <param name="ip"></param>
		public void SendPasswordResetEmail(string ssn, string createdByIP)
		{
			// Check if the user exists
			var user = (from mUser in _myschoolCtx.Users
						where mUser.SSN == ssn
						select mUser).SingleOrDefault();

			if (user == null)
				throw new UserNotFoundException();

			// user exists, proceed by create a record in the database and send email
			var randomKey = this.Token();
			var passRecovery = new PasswordRecovery
			{
				Token = randomKey,
				Username = user.Username,
				TimeStamp = DateTime.Now,
				CreatedByIP = createdByIP
			};
			_passCtx.PasswordRecovery.Add(passRecovery);
			_passCtx.SaveChanges();
		}

		/// <summary>
		/// Given a token it will reset the password of the user that is assigned to that token. 
		/// </summary>
		/// <param name="token"></param>
		/// <param name="usedByIP"></param>
		public void ResetPassword(string token, string usedByIP)
		{
			// TODO
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Creates a random 64 character long token
		/// </summary>
		/// <returns></returns>
		private string Token()
		{
			string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var builder = new StringBuilder();
			for (int i = 0; i < 64; i++)
			{
				var ch = allowedChars[Convert.ToInt32(Math.Floor(allowedChars.Length *_random.NextDouble()))];
				builder.Append(ch);
			}
			return builder.ToString();
		}
		#endregion
	}
}