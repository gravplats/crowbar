namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserResponse"/> type.
    /// </summary>
    public static class BrowserResponseAssertExtensions
    {
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