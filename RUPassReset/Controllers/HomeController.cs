using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RUPassReset.Service;
using RUPassReset.Service.Models.Password;
using RUPassReset.Service.ServiceModels;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		private EmailService _mailService = new EmailService();

		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			return View();
		}

		[HttpGet]
		public ActionResult Forgot()
		{
			var model = new Password();
			return View(model);
		}

		[HttpPost]
		public ActionResult Forgot(Password fpmodel)
		{
			if (ModelState.IsValid)
			{
				_mailService.sendPasswordResetEmail(new User {Email = "benediktl11@ru.is", Name = "Logi"}, "aæsdkfæasdjkf");
				return RedirectToAction("Index");
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
				return RedirectToAction("Index");
			}
			return View(cpmodel);
		}

	}
}
