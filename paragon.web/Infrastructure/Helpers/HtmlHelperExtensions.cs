using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paragon.Parser;

namespace Paragon.Infrastructure
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString ParseKeyword(this HtmlHelper helper, string line)
        {
            var fragment = KeywordGrammar.Instance.ParseString(line);
            var txt = fragment.Text;

            foreach (var key in fragment.Keywords)
            {
                txt = txt.Replace(key, "<a href='#' style='text-decoration: none; border-bottom:1px dashed;cursor:help;'>" + key + "</a>");
            }

            return new HtmlString(txt);
        }
    }
}