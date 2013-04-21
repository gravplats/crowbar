using System;

namespace Crowbar
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Cannot be null or empty.", "value");
            }

            char ch = value[0];
            return char.ToLower(ch) + value.Substring(1);
        }

        public static string RemoveLeadingSlash(this string path)
        {
            if (path.StartsWith("/"))
            {
                return path.Substring(1);
            }

            if (path.StartsWith("~/"))
            {
                return path.Substring(2);
            }

            return path;
        }
    }
}