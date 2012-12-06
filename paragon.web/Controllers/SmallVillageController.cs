using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace Paragon.Web.Controllers
{
    [RoutePrefix("game/{gameid}/world/the-empire/small-village")]
    public class SmallVillageController : Controller
    {
        [GET("")]
        public ActionResult Hub(string gameid)
        {
            return View();
        }

        [GET("secret-meeting")]
        public ActionResult SecretMeeting(string gameid)
        {
            return View();
        }

    }
}
