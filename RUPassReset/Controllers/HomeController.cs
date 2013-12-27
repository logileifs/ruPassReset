using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RUPassReset.Service.Models.Password;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			return View();
		}

		[HttpGet]
		public ActionResult Forgot()
		{
			var model = new ForgotPasswordModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult Forgot(ForgotPasswordModel fpmodel)
		{
			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			return View(fpmodel);
		}

		[HttpGet]
		public ActionResult Change()
		{
			var model = new ChangePasswordModel();
			return View(model);
		}

		[HttpPost]
		public ActionResult Change(ChangePasswordModel cpmodel)
		{
			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
			return View(cpmodel);
		}

	}
}
