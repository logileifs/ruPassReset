using System;
using System.Web.Mvc;
using RUPassReset.Configuration;
using RUPassReset.Service;
using RUPassReset.Service.Exceptions;
using RUPassReset.Service.Helpers;
using RUPassReset.Service.Models.Password;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		#region Private variables
		private PasswordService _passwordService;
		private RegexUtilities _regexUtilities;
		#endregion

		#region Public methods
		public HomeController()
		{
			_passwordService = new PasswordService();
			_regexUtilities = new RegexUtilities();
		}

		public ActionResult Index()
		{
			return View("Reset");
		}

		[HttpGet]
		public ActionResult Reset()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Reset(string email)
		{
			if (!_regexUtilities.IsValidEmail(email))
			{
				ModelState.AddModelError("Error", "This email is invalid.");
				return View();
			}

			try
			{
				var user = _passwordService.CreateResetToken(email, GetIp());
				return View("ResetEmailSent", user);
			}
			catch (UserNotFoundException unfex)
			{
				ModelState.AddModelError("Error", "Could not find any user with that email.");
				return View();
			}
			catch (TooManyTriesException tmtex)
			{
				ModelState.AddModelError("Error", "Too many attempts. Please try again later.");
				return View();
			}
			
		}

		[HttpGet]
		public ActionResult Verify(string token)
		{
			// Check if token is active
			var recovery = _passwordService.VerifyToken(token);
			if (token == null || recovery == null)
			{
				return View("UnableToVerify");
			}
		
			var changePass = new ChangePassword();
			changePass.Token = token;
			changePass.Username = recovery.Username;

			return View("Change", changePass);
		}

		[HttpPost]
		public ActionResult Verify(ChangePassword changePassword)
		{
			// First, check if token is active
			var passRecovery = _passwordService.VerifyToken(changePassword.Token);
			if (passRecovery == null)
				return View("UnableToVerify");

			// Check if modelstate is valid
			if (!ModelState.IsValid)
				return View("Change", changePassword);

			// Check if password is long enough
			if (changePassword.PasswordNew.Length < RUPassResetConfig.Config.MinimumPasswordLength)
			{
				ModelState.AddModelError("Error", String.Format("Password must be longer than {0} characters.", RUPassResetConfig.Config.MinimumPasswordLength));
				return View("Change", changePassword);
			}

			// All checks passed, proceed to change password
			try
			{
				_passwordService.ResetPassword(passRecovery, changePassword.PasswordNew, GetIp());
			}
			catch (IllegalTokenException itex)
			{
				return View("UnableToVerify");
			}
			catch (PasswordResetFailedException prfex)
			{
				return View("Error");
			}
			
			return View("PasswordChangeSuccess");
		}
		#endregion

		#region Private methods

		/// <summary>
		/// Returns the IP address of the request. 
		/// Does not work for localhost. 
		/// </summary>
		/// <returns></returns>
		private string GetIp()
		{
			var ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (!string.IsNullOrEmpty(ipList))
			{
				return ipList.Split(',')[0];
			}
			return Request.ServerVariables["REMOTE_ADDR"];
		}
		#endregion
	}
}
