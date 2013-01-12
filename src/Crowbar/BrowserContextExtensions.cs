using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserContext"/> type.
    /// </summary>
    public static class BrowserContextExtensions
    {
        /// <summary>
        /// Adds a forms authentication cookie to the request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        public static void FormsAuth(this BrowserContext context, string username = "", int timeout = 30)
        {
            var section = ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection;
            if (section == null || section.Mode != AuthenticationMode.Forms)
            {
                throw new InvalidOperationException("The web application is not configured to use forms authentication.");
            }

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
        /// <param name="contentType">The content type of the request.</param>/// 
        public static void JsonBody(this BrowserContext context, object model, string contentType = "application/json")
        {
            string json = Json.Encode(model);
            context.Body(json, contentType);
        }

        /// <summary>
        /// Adds an application/xml request body.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="model">The model that should be converted to XML.</param>
        /// <param name="contentType">The content type of the request.</param>
        public static void XmlBody<TModel>(this BrowserContext context, TModel model, string contentType = "application/xml")
        {
            using (var buffer = new MemoryStream())
            using (var writer = new XmlTextWriter(buffer, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(TModel));
                serializer.Serialize(writer, model);

                writer.Flush();

                byte[] bytes = buffer.ToArray();
                string xml = Encoding.UTF8.GetString(bytes);

                context.Body(xml, contentType);
            }
        }
    }
}