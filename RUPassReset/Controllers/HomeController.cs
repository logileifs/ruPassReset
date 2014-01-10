using System.Web.Mvc;
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
			return View();
		}

		[HttpGet]
		public ActionResult Reset()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Reset(string SSN)
		{
			if (SSN.Length != 10)
			{
				ModelState.AddModelError("Error", "Invalid social security number.");
				return View();
			}

			try
			{
				var ip = "89.160.136.204"; // hardcoded to begin with
				var user = _passwordService.CreateResetToken(SSN, ip);
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
			var recovery = _passwordService.VerifyToken(token);
			if (token == null || recovery == null)
				return View("UnableToVerify");

			var changePass = new ChangePassword();
			changePass.Token = token;
			changePass.Username = recovery.Username;

			return View("Change", changePass);
		}

		[HttpPost]
		public ActionResult Change(ChangePassword changePassword)
		{
			if (!ModelState.IsValid)
				return View("Change", changePassword);
			if (_passwordService.VerifyToken(changePassword.Token) == null)
				return View("UnableToVerify");
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
