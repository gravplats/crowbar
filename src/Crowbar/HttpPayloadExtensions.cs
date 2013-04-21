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
    /// Defines extensions for the <see cref="HttpPayload"/> class.
    /// </summary>
    public static class HttpPayloadExtensions
    {
        /// <summary>
        /// Adds a forms authentication cookie to the request.
        /// </summary>
        /// <param name="payload">The <see cref="HttpPayload"/> that this data should be added to.</param>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        /// <returns>The current HTTP payload.</returns>
        public static HttpPayload FormsAuth(this HttpPayload payload, string username = "", int timeout = 30)
        {
            var section = ConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection;
            if (section == null || section.Mode != AuthenticationMode.Forms)
            {
                throw new InvalidOperationException("The web application is not configured to use forms authentication.");
            }

            // This works because we're encrypting and decrypting on the same machine? In the same AppDomain?
            var ticket = new FormsAuthenticationTicket(username, false, timeout);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            payload.Cookie(cookie);

            return payload;
        }

        /// <summary>
        /// Adds an application/json request body.
        /// </summary>
        /// <param name="payload">The <see cref="HttpPayload"/> that this data should be added to.</param>
        /// <param name="model">The model that should be converted to JSON.</param>
        /// <param name="contentType">The content type of the request.</param>/// 
        /// <returns>The current HTTP payload.</returns>
        public static HttpPayload JsonBody(this HttpPayload payload, object model, string contentType = "application/json")
        {
            string json = Json.Encode(model);
            payload.Body(json, contentType);

            return payload;
        }

        /// <summary>
        /// Adds an application/xml request body.
        /// </summary>
        /// <param name="payload">The <see cref="HttpPayload"/> that this data should be added to.</param>
        /// <param name="model">The model that should be converted to XML.</param>
        /// <param name="contentType">The content type of the request.</param>
        /// <returns>The current HTTP payload.</returns>
        public static HttpPayload XmlBody<TModel>(this HttpPayload payload, TModel model, string contentType = "application/xml")
        {
            using (var buffer = new MemoryStream())
            using (var writer = new XmlTextWriter(buffer, Encoding.UTF8))
            {
                var serializer = new XmlSerializer(typeof(TModel));
                serializer.Serialize(writer, model);

                writer.Flush();

                byte[] bytes = buffer.ToArray();
                string xml = Encoding.UTF8.GetString(bytes);

                payload.Body(xml, contentType);

                return payload;
            }
        }
    }
}