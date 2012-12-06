using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace Paragon.Web.Controllers
{
    [RoutePrefix("game/{gameid}/{name}")]
    public class GameController : Controller
    {
        [GET("tombraider")]
        public ActionResult TombRaider(string gameid, string name)
        {
            return View(new Models.NewGame { GameId = gameid, Name = name, Template = Models.NewGame.Templates.tombraider });
        }


    }
}
