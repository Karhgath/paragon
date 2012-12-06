using System.Web;
using System.Web.Mvc;
using System;

namespace Paragon.Infrastructure
{
    public static class UrlExtensions
    {
        public static string Current(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string AbsoluteContent(this UrlHelper url, string path)
        {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);

            if (!uri.IsAbsoluteUri)
            {
                var builder = new UriBuilder(url.RequestContext.HttpContext.Request.Url) { Path = VirtualPathUtility.ToAbsolute(path) };
                uri = builder.Uri;
            }

            return uri.ToString();
        }


        public static string Login(this UrlHelper url)
        {
            return url.Content("~/login");
        }


        public static string Register(this UrlHelper url)
        {
            return url.Content("~/register");
        }

        public static string Home(this UrlHelper url)
        {
            return url.Content("~/");
        }

        public static string Profile(this UrlHelper url)
        {
            return url.Content("~/profile");
        }

        public static string Start(this UrlHelper url)
        {
            return url.Content("~/start");
        }


        public static string SmallVillage(this UrlHelper url, string gameid)
        {
            return url.Content("~/game/" + gameid + "/world/the-empire/small-village");
        }
    }
}