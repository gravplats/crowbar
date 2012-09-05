using System.Web;
using System.Web.Helpers;
using System.Web.Security;

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