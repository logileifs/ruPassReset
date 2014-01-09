using System.Web.Mvc;
using RUPassReset.Service;
using RUPassReset.Service.Models.Password;
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		private PasswordService _passwordService;

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
		public ActionResult Reset(Password fpmodel)
		{
			if (ModelState.IsValid)
			{
				var test = _passwordService.reset();
				return View("ResetEmailSent", fpmodel);
			}
			return View(fpmodel);
		}

	}
}
