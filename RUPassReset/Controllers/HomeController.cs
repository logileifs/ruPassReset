using System;
using System.Web.Mvc;
using RUPassReset.Configuration;
using RUPassReset.Service;
using RUPassReset.Service.Models.Password;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		#region Private variables
		private PasswordService _passwordService;
		#endregion

		#region Public methods
		public HomeController()
		{
			_passwordService = new PasswordService();
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
		public ActionResult Reset(string SSN)
		{
			if (SSN.Length != 11)
			{
				ModelState.AddModelError("Error", "Invalid social security number.");
				return View();
			}

			var sanitizedSSN = SSN.Replace("-", "");

			try
			{
				var user = _passwordService.CreateResetToken(sanitizedSSN, "89.160.136.204");
				return View("ResetEmailSent", user);
			}
			catch (UserNotFoundException unfex)
			{
				ModelState.AddModelError("Error", "User not found.");
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
			{
				return View("UnableToVerify");
			}
			// Check if modelstate is valid
			if (!ModelState.IsValid)
			{
				return View("Change", changePassword);
			}
			// Check if password is long enough
			if (changePassword.PasswordNew.Length < RUPassResetConfig.Config.MinimumPasswordLength)
			{
				ModelState.AddModelError("Error", String.Format("Password must be longer than {0} characters.", RUPassResetConfig.Config.MinimumPasswordLength));
				return View("Change", changePassword);
			}

			// All checks passed, proceed to change password
			try
			{
				_passwordService.ResetPassword(passRecovery, changePassword.PasswordNew, "89.160.136.204");
			}
			catch (IllegalTokenException itex)
			{
				return View("UnableToVerify");
			}
			
			return View("PasswordChangeSuccess");
		}
		#endregion

		#region Private methods

		/// <summary>
		/// Returns the IP address of the request. 
		/// Currently not working.
		/// </summary>
		/// <returns></returns>
		private string GetIp()
		{
			string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (!string.IsNullOrEmpty(ipList))
			{
				return ipList.Split(',')[0];
			}
			return Request.ServerVariables["REMOTE_ADDR"];
		}
		#endregion
	}
}
