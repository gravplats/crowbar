using System;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserResponse"/> type.
    /// </summary>
    public static class BrowserResponseAssertExtensions
    {
        /// <summary>
        /// Asserts that response is of type 'application/json'.
        /// </summary>
        /// <param name="response">The <see cref="BrowserResponse"/> that the assert should be made on.</param>
        /// <param name="assertions">Additional assertions on the JSON object.</param>
        /// <returns>The JSON object.</returns>
        public static dynamic ShouldBeJson(this BrowserResponse response, Action<dynamic> assertions = null)
        {
            if (response.Response.ContentType != "application/json")
            {
                throw new AssertException("The content type is not application/json.");
            }

            try
            {
                dynamic json = response.AsJson();
                if (assertions != null)
                {
                    assertions(json);
                }

                return json;
            }
            catch (Exception exception)
            {
                throw new AssertException("Failed to convert response body into JSON.", exception);
            }
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