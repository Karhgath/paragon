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
    [RoutePrefix("game/{gameid}/world/{realm}/{hub}")]
    public class HubController : Controller
    {
        [GET("")]
        public ActionResult Hub(string gameid, string realm, string hub)
        {
            var file = new Infrastructure.FileHandler(Server);
            var text = file.ReadHub(realm, hub);

            var vm = Parser.HubGrammar.Instance.ParseString(text);
            
            return View(vm);
        }

        [POST("")]
        public ActionResult Hub(string gameid, string realm, string hub, string action)
        {
            var file = new Infrastructure.FileHandler(Server);
            var text = file.ReadHub(realm, hub);

            var vm = Parser.HubGrammar.Instance.ParseString(text);

            if (!vm.Options.Any(x => x.Action == action))
            {
                // ERROR?
                return Redirect(Request.Url.AbsoluteUri);
            }

            var parts = action.Split('/');

            switch (parts[0])
            {
                case "event":
                    return Redirect(Request.Url.AbsoluteUri);
                case "shop":
                    return Redirect(Request.Url.AbsoluteUri);
                case "visit":
                    if (!vm.Locations.Any(x => x == parts[1]))
                    {
                        return Redirect(Request.Url.AbsoluteUri);
                    }
                    return Redirect(Request.Url.Child(parts[1]).ToAbsolute());

                default:
                    return Redirect(Request.Url.AbsoluteUri);
            }
        }

        [GET("secret-meeting")]
        public ActionResult SecretMeeting(string gameid, string realm, string hub)
        {
            return View();
        }


        [GET("inn")]
        public ActionResult Inn(string gameid, string realm, string hub)
        {
            return View("InnSuspicious");
        }

    }
}
