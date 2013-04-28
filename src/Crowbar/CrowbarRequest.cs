using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Represents the request of the HTTP worker.
    /// </summary>
    public class CrowbarRequest
    {
        private readonly ICrowbarHttpWorkerContext context;

        /// <summary>
        /// Creates an instance of <see cref="CrowbarRequest"/>.
        /// <param name="path">The requested path.</param>
        /// <param name="context">The request context.</param>
        /// </summary>
        public CrowbarRequest(string path, ICrowbarHttpWorkerContext context)
        {
            Path = path;
            this.context = context;
        }

        /// <summary>
        /// Gets the HTTP cookies.
        /// </summary>
        public HttpCookieCollection Cookies
        {
            get { return context.Cookies; }
        }

        /// <summary>
        /// Gets the form values.
        /// </summary>
        public NameValueCollection FormValues
        {
            get { return context.FormValues; }
        }

        /// <summary>
        /// Gets the HTTP headers.
        /// </summary>
        public NameValueCollection Headers
        {
            get { return context.Headers; }
        }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        public string Method
        {
            get { return context.Method; }
        }

        /// <summary>
        /// Gets the requested path.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        public string Protocol
        {
            get { return context.Protocol; }
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        public string QueryString
        {
            get { return context.QueryString; }
        }

        /// <summary>
        /// Gets the request body.
        /// </summary>
        public string RequestBody
        {
            get { return context.BodyString; }
        }
    }
}