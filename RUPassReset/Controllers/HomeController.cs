using System.Web.Mvc;
using RUPassReset.Service;

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
				_passwordService.SendPasswordResetEmail(SSN, ip);
				return View("ResetEmailSent");
			}
			catch (UserNotFoundException unfex)
			{
				ModelState.AddModelError("Error", "User not found.");
				return View();
			}
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
