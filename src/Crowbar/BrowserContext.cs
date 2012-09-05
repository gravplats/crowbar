using System;
using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    public class BrowserContext : ISimulatedWorkerRequestContext
    {
        private readonly ISimulatedWorkerRequestContext context;

        public BrowserContext(string method)
        {
            context = this;
            context.FormValues = new NameValueCollection();
            context.Headers = new NameValueCollection();
            context.Method = method;
            context.Protocol = "http";
            context.QueryString = string.Empty;

            context.Headers["Content-Type"] = "application/octet-stream";
        }

        string ISimulatedWorkerRequestContext.BodyString { get; set; }

        HttpCookieCollection ISimulatedWorkerRequestContext.Cookies { get; set; }

        NameValueCollection ISimulatedWorkerRequestContext.FormValues { get; set; }

        NameValueCollection ISimulatedWorkerRequestContext.Headers { get; set; }

        string ISimulatedWorkerRequestContext.Method { get; set; }

        string ISimulatedWorkerRequestContext.Protocol { get; set; }

        string ISimulatedWorkerRequestContext.QueryString { get; set; }

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
        public void Query(string key, string value)
        {
            context.QueryString += string.Format("{0}{1}={2}", context.QueryString.Length == 0 ? "" : "&", key, value);
        }
    }
}