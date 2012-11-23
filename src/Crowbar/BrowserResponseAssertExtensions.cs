using System;
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

            if (assertions != null)
            {
                assertions(document);
            }

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
            response.AssertContentType("application/json");

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

            if (assertions != null)
            {
                assertions(xml);
            }

            return xml;
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
                string message = string.Format("Location should have been '{0}' but was '{1}'.", location, response.Headers["Location"]);
                throw new AssertException(message);
            }
        }

        private static void AssertContentType(this BrowserResponse response, string expectedContentType)
        {
            if (response.ContentType != expectedContentType)
            {
                string message = string.Format("The content type should have been '{0}' but was '{1}'.", expectedContentType, response.ContentType);
                throw new AssertException(message);
            }
        }

        private static void AssertStatusCode(this BrowserResponse response, HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode != expectedStatusCode)
            {
                string message = string.Format("HTTP status code should have been '{0}' but was '{1}'.", expectedStatusCode, response.StatusCode);
                throw new AssertException(message);
            }
        }
    }
}