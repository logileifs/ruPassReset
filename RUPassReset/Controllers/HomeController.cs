using System.Web.Mvc;
using RUPassReset.Service;
using RUPassReset.Service.Models.Password;
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		private PasswordService _passwordService = new PasswordService();

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
				return View("ResetEmailSent", fpmodel);
			}
			return View(fpmodel);
		}

		[HttpGet]
		public ActionResult Change()
		{
			var model = new ChangePassword();
			return View(model);
		}

		[HttpPost]
		public ActionResult Change(ChangePassword cpmodel)
		{
			if (ModelState.IsValid)
			{
				var user = new User
				{
					Email = "benediktl11@ru.is",
					Username = "benediktl11"
				};
				_passwordService.changePassword(user, "kartafla");
				return RedirectToAction("Index");
			}
			return View(cpmodel);
		}

	}
}
