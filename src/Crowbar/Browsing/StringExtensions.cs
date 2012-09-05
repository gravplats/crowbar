namespace Crowbar.Browsing
{
    internal static class StringExtensions
    {
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