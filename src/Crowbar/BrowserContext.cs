using System;
using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Defines the context that a <see cref="Browser"/> instance should run under.
    /// </summary>
    public class BrowserContext : ISimulatedWorkerRequestContext
    {
        private readonly ISimulatedWorkerRequestContext context;

        /// <summary>
        /// Creates an instance of <see cref="BrowserContext"/>.
        /// </summary>
        /// <param name="mvcMajorVersion">The major version of the MVC framework</param>
        /// <param name="method">The HTTP method.</param>
        public BrowserContext(int mvcMajorVersion, string method)
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
        public void AjaxRequest()
        {
            Header("X-Requested-With", "XMLHttpRequest");
        }

        /// <summary>
        /// Adds a body to the request.
        /// </summary>
        /// <param name="body">A string that should be used as the request body.</param>
        /// <param name="contentType">The content type.</param>
        public void Body(string body, string contentType = "application/octet-stream")
        {
            context.BodyString = body;
            context.Headers["Content-Type"] = contentType;
        }

        /// <summary>
        /// Adds a cookie to the request.
        /// </summary>
        /// <param name="cookie">The cookie that should be added.</param>
        public void Cookie(HttpCookie cookie)
        {
            Ensure.NotNull(cookie, "cookie");
            context.Cookies.Add(cookie);
        }

        /// <summary>
        /// Adds a form value to the request.
        /// </summary>
        /// <param name="key">The name of the form element.</param>
        /// <param name="value">The value of the form element.</param>
        public void FormValue(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(context.BodyString))
            {
                throw new InvalidOperationException("Form value cannot be set as well as body string.");
            }

            context.FormValues.Add(key, value);
        }

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="value">The name of the value.</param>
        public void Header(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Missing name of header.", "name");
            }

            context.Headers.Add(name, value);
        }

        /// <summary>
        /// Configures the request to be sent over HTTP.
        /// </summary>
        public void HttpRequest()
        {
            context.Protocol = "http";
        }

        /// <summary>
        /// Configures the request to be sent over HTTPS.
        /// </summary>
        public void HttpsRequest()
        {
            context.Protocol = "https";
        }

        /// <summary>
        /// Adds a query string entry.
        /// </summary>
        /// <param name="key">The name of the entry.</param>
        /// <param name="value">The value of the entry.</param>
        public void QueryString(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Missing key of query parameter.", "key");
            }

            context.QueryString += string.Format("{0}{1}={2}", context.QueryString.Length == 0 ? "" : "&", key, value);
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