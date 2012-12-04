using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace paragon.web.Controllers
{
	[RoutePrefix("")]
    public class HomeController : Controller
    {
		[GET("")]
        public ActionResult Index()
        {
            return View();        
        }

        [GET("login")]
        public ActionResult Login()
        {
            return View();
        }

        [GET("profile")]
        public ActionResult Profile()
        {
            return View();
        }

        [GET("register")]
        public ActionResult Register()
        {
            return View();
        }
    }
}
