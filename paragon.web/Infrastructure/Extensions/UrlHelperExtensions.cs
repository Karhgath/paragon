using System.Web;
using System.Web.Mvc;
using System;
using System.Linq;
using Paragon.Infrastructure;

namespace Paragon.Infrastructure
{
    public static class UrlHelperExtensions
    {
        public static string Current(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.Request.RawUrl;
        }

        public static string Back(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.Request.Url.Parent().ToAbsolute();
        }

        public static string Current(this UrlHelper url, string path)
        {
            var builder = new UriBuilder(url.RequestContext.HttpContext.Request.Url);

            var tokens = path.Trim('/').Split('/');

            foreach (var token in tokens.SkipWhile(x => x == "."))
            {
                if (token == "..")
                {
                    builder = builder.Parent();
                }
                else
                {
                    builder = builder.Child(token);
                }
            }

            return builder.Uri.AbsoluteUri;
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