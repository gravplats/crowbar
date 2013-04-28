using System.Collections.Specialized;

namespace Crowbar
{
    /// <summary>
    /// Represents the response of the HTTP worker request.
    /// </summary>
    public class CrowbarResponse
    {
        private readonly NameValueCollection headers;

        /// <summary>
        /// Creates an instance of <see cref="Client"/>.
        /// </summary>
        public CrowbarResponse()
        {
            headers = new NameValueCollection();
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets the HTTP status description.
        /// </summary>
        public string StatusDescription { get; internal set; }

        /// <summary>
        /// Returns the HTTP headers for the response.
        /// </summary>
        /// <returns>The HTTP headers.</returns>
        public NameValueCollection GetHeaders()
        {
            return headers;
        }

        /// <summary>
        /// Adds an HTTP headers to the response.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        public void AddHeader(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            headers.Add(name, value);
        }
    }
}