using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Paragon.Web.Infrastructure;

namespace Paragon.Web.Controllers
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

        [GET("start")]
        public ActionResult Start()
        {
            return View();
        }

        [POST("start")]
        public ActionResult Start(Models.NewGame newgame)
        {
            if (string.IsNullOrEmpty(newgame.Name)) newgame.Name = "Anonymous";

            if (newgame.Template == Models.NewGame.Templates.tombraider)
            {
                return RedirectToRoute("Game_TombRaider", new { name = newgame.Name, gameid = new ShortGuid(Guid.NewGuid()) });
            }

            return RedirectToAction("Start");
        }
    }
}
