using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RUPassReset.Models;

namespace RUPassReset.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			return View();
		}

		public ActionResult Forgot()
		{
			return View();
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
