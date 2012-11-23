using System;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using Crowbar.AspNetMvc;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserContext"/> type.
    /// </summary>
    public static class BrowserContextExtensions
    {
        /// <summary>
        /// Adds a header indicating that this is an AJAX request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        public static void AjaxRequest(this BrowserContext context)
        {
            context.Header("X-Requested-With", "XMLHttpRequest");
        }

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

            if (!String.IsNullOrEmpty(path))
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

        /// <summary>
        /// Adds a forms authentication cookie to the request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        public static void FormsAuth(this BrowserContext context, string username = "", int timeout = 30)
        {
            // This works because we're encrypting and decrypting on the same machine? In the same AppDomain?
            var ticket = new FormsAuthenticationTicket(username, false, timeout);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            context.Cookie(cookie);
        }

        /// <summary>
        /// Adds an application/json request body.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="model">The model that should be converted to JSON.</param>
        public static void JsonBody(this BrowserContext context, object model)
        {
            string json = Json.Encode(model);
            context.Body(json, "application/json");
        }
    }
}