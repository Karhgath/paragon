using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paragon.Infrastructure
{
    public static class StringExtensions
    {
        public static bool IsBlank(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}