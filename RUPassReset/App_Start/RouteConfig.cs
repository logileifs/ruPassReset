using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RUPassReset
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Reset",
				url: "reset/",
				defaults: new { controller = "Home", action = "Reset" }
			);

			routes.MapRoute(
				name: "Verify",
				url: "verify/",
				defaults: new { controller = "Home", action = "Verify" }
			);

			routes.MapRoute(
				name: "Change",
				url: "change/",
				defaults: new { controller = "Home", action = "Change" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}