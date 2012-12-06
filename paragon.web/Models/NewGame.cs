using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paragon.Web.Models
{
    public class NewGame
    {
        public string GameId { get; set; }
        public string Name { get; set; }
        public Templates Template { get; set; }

        public enum Templates
        {
            tombraider,
            barbarian,
            bard
        }
    }
}