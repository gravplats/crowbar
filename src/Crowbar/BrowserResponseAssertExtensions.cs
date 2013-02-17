using System;
using System.Web;
using System.Xml.Linq;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserResponse"/> type.
    /// </summary>
    public static class BrowserResponseAssertExtensions
    {
        /// <summary>
        /// Asserts that the response content type is of the MIME-type 'text/html'.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="assertions">Additional assertions on the CQ object.</param>
        /// <returns>The CQ object.</returns>
        public static CQ ShouldBeHtml(this BrowserResponse response, Action<CQ> assertions = null)
        {
            response.AssertStatusCode(HttpStatusCode.OK);
            response.AssertContentType("text/html");

            CQ document;

            try
            {
                document = response.AsCsQuery();
            }
            catch (Exception exception)
            {
                throw new AssertException("Failed to convert response body into a CQ object.", exception);
            }

            assertions.TryInvoke(document);

            return document;
        }

        /// <summary>
        /// Asserts that the response content type is of the MIME-type 'application/json' or the specified override.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="assertions">Additional assertions on the JSON object.</param>
        /// <param name="contentType">The expected content type.</param>
        /// <returns>The JSON object.</returns>
        public static dynamic ShouldBeJson(this BrowserResponse response, Action<dynamic> assertions = null, string contentType = "application/json")
        {
            response.AssertStatusCode(HttpStatusCode.OK);
            response.AssertContentType(contentType);

            dynamic json;

            try
            {
                json = response.AsJson();
            }
            catch (Exception exception)
            {
                throw new AssertException("Failed to convert response body into JSON.", exception);
            }

            if (assertions != null)
            {
                assertions(json);
            }

            return json;
        }

        /// <summary>
        /// Asserts that the response content type is of the MIME-type 'application/xml' or the specified override.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="assertions">Additional assertions on the XML object.</param>
        /// <param name="contentType">The expected content type.</param>
        /// <returns>An XElement.</returns>
        public static XElement ShouldBeXml(this BrowserResponse response, Action<XElement> assertions = null, string contentType = "application/xml")
        {
            response.AssertStatusCode(HttpStatusCode.OK);
            response.AssertContentType(contentType);

            XElement xml;

            try
            {
                xml = response.AsXml();
            }
            catch (Exception exception)
            {
                throw new AssertException("Failed to convert response body into XML.", exception);
            }

            assertions.TryInvoke(xml);

            return xml;
        }

        /// <summary>
        /// Assert that the response has a cookie with the specified name.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="name">The name of the cookie.</param>
        /// <returns>The cookie.</returns>
        public static HttpCookie ShouldHaveCookie(this BrowserResponse response, string name)
        {
            Ensure.NotNullOrEmpty(name, "name");

            if (response.HttpResponse == null)
            {
                throw new InvalidOperationException("The HttpResponse is null.");
            }

            var cookie = response.HttpResponse.Cookies[name];
            if (cookie == null)
            {
                throw new AssertException("Missing cookie '{0}'.", name);
            }

            return cookie;
        }

        /// <summary>
        /// Asserts that the response has a cookie with the specified name and value.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <returns>The cookie.</returns>
        public static HttpCookie ShouldHaveCookie(this BrowserResponse response, string name, string value)
        {
            var cookie = response.ShouldHaveCookie(name);
            if (!string.Equals(cookie.Value, value, StringComparison.Ordinal))
            {
                throw new AssertException("The value of cookie '{0}' should have been '{1}' but was '{2}'.", name, value, cookie.Value);
            }

            return cookie;
        }

        /// <summary>
        /// Asserts that a permanent redirect to a specified location took place.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="location">The location that should have been redirected to.</param>
        public static void ShouldHavePermanentlyRedirectTo(this BrowserResponse response, string location)
        {
            response.ShouldHaveRedirectedTo(location, HttpStatusCode.MovedPermanently);
        }

        /// <summary>
        /// Asserts that a temporarily redirect to a specified location took place.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="location">The location that should have been redirected to.</param>
        public static void ShouldHaveTemporarilyRedirectTo(this BrowserResponse response, string location)
        {
            response.ShouldHaveRedirectedTo(location, HttpStatusCode.Found);
        }

        private static void ShouldHaveRedirectedTo(this BrowserResponse response, string location, HttpStatusCode expectedStatusCode)
        {
            response.AssertStatusCode(expectedStatusCode);

            if (response.Headers["Location"] != location)
            {
                throw new AssertException("Location should have been '{0}' but was '{1}'.", location, response.Headers["Location"]);
            }
        }

        private static void AssertContentType(this BrowserResponse response, string expectedContentType)
        {
            if (response.ContentType != expectedContentType)
            {
                throw new AssertException("The content type should have been '{0}' but was '{1}'.", expectedContentType, response.ContentType);
            }
        }

        private static void AssertStatusCode(this BrowserResponse response, HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode != expectedStatusCode)
            {
                throw new AssertException("HTTP status code should have been '{0}' but was '{1}'.", expectedStatusCode, response.StatusCode);
            }
        }
    }
}