using System;
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
            if (response.ContentType != "text/html")
            {
                throw new AssertException("The content type is not text/html.");
            }

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
        /// Asserts that the response content type is of the MIME-type 'application/json'.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="assertions">Additional assertions on the JSON object.</param>
        /// <returns>The JSON object.</returns>
        public static dynamic ShouldBeJson(this BrowserResponse response, Action<dynamic> assertions = null)
        {
            if (response.ContentType != "application/json")
            {
                throw new AssertException("The content type is not application/json.");
            }

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
        /// Asserts that a permanent redirect to a specified location took place.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="location">The location that should have been redirected to.</param>
        public static void ShouldHavePermanentlyRedirectTo(this BrowserResponse response, string location)
        {
            var code = HttpStatusCode.MovedPermanently;
            response.ShouldHaveRedirectedTo(location, code);
        }

        /// <summary>
        /// Asserts that a temporarily redirect to a specified location took place.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="location">The location that should have been redirected to.</param>
        public static void ShouldHaveTemporarilyRedirectTo(this BrowserResponse response, string location)
        {
            var code = HttpStatusCode.Found;
            response.ShouldHaveRedirectedTo(location, code);
        }

        private static void ShouldHaveRedirectedTo(this BrowserResponse response, string location, HttpStatusCode code)
        {
            if (response.StatusCode != code)
            {
                string message = string.Format("HTTP status code should have been '{0}' but was '{1}'.", code, response.StatusCode);
                throw new AssertException(message);
            }

            if (response.Headers["Location"] != location)
            {
                string message = string.Format("Location should have been '{0}' but was '{1}'.", location, response.Headers["Location"]);
                throw new AssertException(message);
            }
        }
    }
}