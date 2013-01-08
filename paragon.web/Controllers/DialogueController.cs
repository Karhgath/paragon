using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Paragon.Infrastructure;

namespace Paragon.Web.Controllers
{
    [RoutePrefix("game/{gameid}/world/{realm}/{hub}/{location}/{zone}/{character}")]
    public class DialogueController : Controller
    {
        [GET("")]
        public ActionResult Greetings(string gameid, string realm, string hub, string location, string zone, string character)
        {
            return View();
        }

        [GET("{tone}")]
        public ActionResult Tone(string gameid, string realm, string hub, string location, string zone, string character)
        {
            return View();
        }

        [GET("{tone}/about")]
        public ActionResult About(string gameid, string realm, string hub, string location, string zone, string character)
        {
            return View();
        }

        [GET("{tone}/about/{keyword}")]
        public ActionResult Keyword(string gameid, string realm, string hub, string location, string zone, string character)
        {
            return View();
        }
        
    }
}
