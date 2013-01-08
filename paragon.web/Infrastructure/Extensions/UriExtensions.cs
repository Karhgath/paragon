using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Paragon.Infrastructure
{
    public static class UriExtensions
    {
        public static UriBuilder NoSlash(this Uri uri)
        {
            var builder = new UriBuilder(uri).NoSlash();

            return builder;
        }

        public static UriBuilder NoSlash(this UriBuilder builder)
        {
            builder.Path.TrimEnd('/');

            return builder;
        }

        public static IEnumerable<Uri> Decompose(this Uri uri, Uri baseUri, bool includeCurrent = false)
        {
            if (!uri.AbsoluteUri.Contains(baseUri.AbsoluteUri)) throw new ArgumentException(string.Format("Path '{0}' is not a child of base path '{1}'", uri.AbsoluteUri, baseUri.AbsoluteUri));

            var basePath = baseUri.NoSlash().Path;
            var builder = uri.NoSlash();

            var stack = new Stack<Uri>();

            if (includeCurrent) stack.Push(builder.Uri);

            while (builder.TryParent() && builder.Path.Contains(basePath))
            {
                stack.Push(builder.Uri);
            }

            return stack;
        }

        public static IEnumerable<string> DecomposeRaw(this Uri uri, Uri baseUri, bool includeCurrent = false)
        {
            return uri.Decompose(baseUri, includeCurrent).Select(x => x.AbsoluteUri);
        }


        public static IEnumerable<KeyValuePair<string, string>> DecomposeKeyValue(this Uri uri, Uri baseUri, bool includeCurrent = false)
        {
            return uri.Decompose(baseUri, includeCurrent).Select(x => new KeyValuePair<string, string>(x.CurrentToken(), x.AbsoluteUri));
        }

        public static Dictionary<string, string> DecomposeDictionary(this Uri uri, Uri baseUri, bool includeCurrent = false)
        {
            return uri.Decompose(baseUri, includeCurrent).ToDictionary(x => x.CurrentToken(), x => x.AbsoluteUri);
        }

        public static UriBuilder Parent(this Uri uri, bool keepQuery = false, bool keepFragment = false)
        {
            var builder = new UriBuilder(uri);

            if (!keepQuery) builder.Query = "";
            if (!keepFragment) builder.Fragment = "";

            return builder.Parent();
        }

        public static UriBuilder Parent(this UriBuilder builder)
        {
            builder.Path = builder.Path.Parent();

            return builder;
        }

        public static string Pop(this Uri uri)
        {
            string current;
            var builder = new UriBuilder(uri);
            builder.Path.Parent(out current);

            return current;
        }

        public static string Pop(this UriBuilder builder)
        {
            string current;
            builder.Path.Parent(out current);

            return current;
        }

        private static bool TryParent(this UriBuilder builder)
        {
            if (builder.Path == "/") return false;

            builder.Path = builder.Path.Parent();

            return true;
        }

        private static string Parent(this string path)
        {
            string current;
            
            return path.Parent(out current);
        }

        private static string Parent(this string path, out string current)
        {
            var tokens = path.Trim('/').Split('/').ToList();

            var sb = new StringBuilder("/");

            current = tokens.LastOrDefault() ?? "";

            foreach (var token in tokens.Take(tokens.Count - 1))
            {
                sb.Append('/');
                sb.Append(token);
            }

            return sb.ToString();
        }

        public static string CurrentToken(this Uri uri)
        {
            var builder = new UriBuilder(uri);
            var tokens = builder.Path.Trim('/').Split('/');

            return tokens.Any() ? tokens[tokens.Length - 1] : "";
        }

        public static string[] Tokens(this Uri uri)
        {
            return new UriBuilder(uri).Tokens();
        }

        public static string[] Tokens(this UriBuilder builder)
        {
            var tokens = builder.Path.Trim('/').Split('/');

            return tokens;
        }

        public static UriBuilder Replace(this UriBuilder builder, string token, bool keepQuery = false, bool keepFragment = false)
        {
            builder.Pop();
            builder.Child(token);

            return builder;
        }

        public static UriBuilder Child(this Uri uri, string token, bool keepQuery = false, bool keepFragment = false)
        {
            var builder = new UriBuilder(uri);

            if (!keepQuery) builder.Query = "";
            if (!keepFragment) builder.Fragment = "";

            return builder.Children(token);
        }

        public static UriBuilder Child(this UriBuilder builder, string token)
        {
            return builder.Children(token);
        }

        public static UriBuilder Children(this Uri uri, params string[] tokens)
        {
            var builder = new UriBuilder(uri);

            builder.Query = "";
            builder.Fragment = "";

            return builder.Children(tokens);
        }

        public static UriBuilder Children(this UriBuilder builder, params string[] tokens)
        {
            IEnumerable<string> validTokens = tokens.Select(x => x.Trim('/')).SkipWhile(x => x == ".");

            if (validTokens.Any(x => x == ".."))
            {
                var queue = new Queue<string>();

                foreach (var token in validTokens)
                {
                    if (token == "..")
                    {
                        if (queue.Any())
                            queue.Dequeue();
                        else
                            builder.Pop();
                    }
                    else
                    {
                        queue.Enqueue(token);
                    }
                }

                return builder.Children(queue.AsEnumerable());
            }

            return builder.Children(validTokens.AsEnumerable());
        }

        private static UriBuilder Children(this UriBuilder builder, IEnumerable<string> tokens)
        {
            var sb = new StringBuilder(builder.Path);

            foreach (var token in tokens)
            {
                sb.Append('/');
                sb.Append(HttpUtility.UrlPathEncode(token));
            }

            builder.Path = sb.ToString();

            return builder;
        }

        public static string ToAbsolute(this UriBuilder builder)
        {
            return builder.Uri.AbsoluteUri;
        }
    }
}