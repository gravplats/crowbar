using System.Web;
using Crowbar.AspNetMvc;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserContext"/> type.
    /// </summary>
    public static class AntiForgeryBrowserExtensions
    {
        /// <summary>
        /// Adds an anti-forgery request token to the request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="username">The username.</param>
        /// <param name="salt">The salt string.</param>
        /// <param name="domain">The domain of the web application that the request is submitted from.</param>
        /// <param name="path">The virtual root path of the web application that the request is submitted from.</param>
        /// <param name="applicationPath">The application path.</param>
        public static void AntiForgeryRequestToken(this BrowserContext context, string username = "", string salt = "", string domain = null, string path = null, string applicationPath = "/")
        {
            var serializer = new AntiForgeryDataSerializer();

            var cookieToken = AntiForgeryData.NewToken();
            string cookieValue = serializer.Serialize(cookieToken);

            string cookieName = AntiForgeryData.GetAntiForgeryTokenName(applicationPath);
            var cookie = new HttpCookie(cookieName, cookieValue) { Domain = null, HttpOnly = true };

            if (!string.IsNullOrEmpty(path))
            {
                cookie.Path = path;
            }

            context.Cookie(cookie);

            var formToken = new AntiForgeryData(cookieToken)
            {
                Salt = salt,
                Username = username
            };

            string formValue = serializer.Serialize(formToken);
            context.FormValue(AntiForgeryData.AntiForgeryTokenFieldName, formValue);
        }
    }
}