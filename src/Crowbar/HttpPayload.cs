using System;
using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Defines an HTTP payload.
    /// </summary>
    public class HttpPayload : ISimulatedWorkerRequestContext
    {
        private readonly ISimulatedWorkerRequestContext context;

        /// <summary>
        /// Creates an instance of <see cref="HttpPayload"/>.
        /// </summary>
        /// <param name="mvcMajorVersion">The major version of the MVC framework</param>
        /// <param name="method">The HTTP method.</param>
        public HttpPayload(int mvcMajorVersion, string method)
        {
            MvcMajorVersion = mvcMajorVersion;
            context = this;
            context.Cookies = new HttpCookieCollection();
            context.FormValues = new NameValueCollection();
            context.Headers = new NameValueCollection();
            context.Method = method;
            context.Protocol = "http";
            context.QueryString = string.Empty;

            context.Headers["Content-Type"] = "application/octet-stream";
        }

        internal int MvcMajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        string ISimulatedWorkerRequestContext.BodyString { get; set; }

        /// <summary>
        /// Gets or sets cookies.
        /// </summary>
        HttpCookieCollection ISimulatedWorkerRequestContext.Cookies { get; set; }

        /// <summary>
        /// Gets or sets the form values.
        /// </summary>
        NameValueCollection ISimulatedWorkerRequestContext.FormValues { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        NameValueCollection ISimulatedWorkerRequestContext.Headers { get; set; }

        /// <summary>
        /// Gets or set the HTTP method.
        /// </summary>
        string ISimulatedWorkerRequestContext.Method { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        string ISimulatedWorkerRequestContext.Protocol { get; set; }

        /// <summary>
        /// Gets or sets the query string.
        /// </summary>
        string ISimulatedWorkerRequestContext.QueryString { get; set; }

        /// <summary>
        /// Adds a header indicating that this is an AJAX request.
        /// </summary>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload AjaxRequest()
        {
            Header("X-Requested-With", "XMLHttpRequest");
            return this;
        }

        /// <summary>
        /// Adds a body to the request.
        /// </summary>
        /// <param name="body">A string that should be used as the request body.</param>
        /// <param name="contentType">The content type.</param>
        public HttpPayload Body(string body, string contentType = "application/octet-stream")
        {
            context.BodyString = body;
            context.Headers["Content-Type"] = contentType;

            return this;
        }

        /// <summary>
        /// Adds a cookie to the request.
        /// </summary>
        /// <param name="cookie">The cookie that should be added.</param>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload Cookie(HttpCookie cookie)
        {
            Ensure.NotNull(cookie, "cookie");
            context.Cookies.Add(cookie);

            return this;
        }

        /// <summary>
        /// Adds a form value to the request.
        /// </summary>
        /// <param name="key">The name of the form element.</param>
        /// <param name="value">The value of the form element.</param>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload FormValue(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(context.BodyString))
            {
                throw new InvalidOperationException("Form value cannot be set as well as body string.");
            }

            context.FormValues.Add(key, value);

            return this;
        }

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The name of the value.</param>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload Header(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Missing name of header.", "name");
            }

            context.Headers.Add(name, value);

            return this;
        }

        /// <summary>
        /// Configures the request to be sent over HTTP.
        /// </summary>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload HttpRequest()
        {
            context.Protocol = "http";
            return this;
        }

        /// <summary>
        /// Configures the request to be sent over HTTPS.
        /// </summary>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload HttpsRequest()
        {
            context.Protocol = "https";
            return this;
        }

        /// <summary>
        /// Adds a query string entry.
        /// </summary>
        /// <param name="key">The name of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        /// <returns>The current HTTP payload.</returns>
        public HttpPayload QueryString(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Missing key of query parameter.", "key");
            }

            context.QueryString += string.Format("{0}{1}={2}", context.QueryString.Length == 0 ? "" : "&", key, value);
            return this;
        }

        internal string ExtractQueryString(string path)
        {
            int querySeparatorIndex = path.IndexOf("?");
            if (querySeparatorIndex >= 0)
            {
                string queryString = path.Substring(querySeparatorIndex + 1);

                // This might fail horribly.
                string[] kvps = queryString.Split('&');
                foreach (string kvp in kvps)
                {
                    string[] parameter = kvp.Split('=');
                    QueryString(parameter[0], parameter[1]);
                }

                path = path.Substring(0, querySeparatorIndex);
            }

            return path;
        }
    }
}