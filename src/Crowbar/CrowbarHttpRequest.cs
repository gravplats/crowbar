using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Represents the request of an HTTP worker.
    /// </summary>
    public class CrowbarHttpRequest : ICrowbarHttpRequest
    {
        private readonly IHttpPayload payload;

        /// <summary>
        /// Creates an instance of <see cref="CrowbarHttpRequest"/>.
        /// <param name="path">The requested path.</param>
        /// <param name="payload">The payload.</param>
        /// </summary>
        public CrowbarHttpRequest(string path, IHttpPayload payload)
        {
            Path = path;
            this.payload = payload;
        }

        /// <inheritdoc />
        public string Path { get; private set; }

        /// <inheritdoc />
        public string QueryString
        {
            get { return payload.QueryString; }
        }

        /// <inheritdoc />
        public string GetRequestBody()
        {
            if (!string.IsNullOrEmpty(payload.RequestBody))
            {
                return payload.RequestBody;
            }

            if (payload.FormValues.Count > 0)
            {
                var entries = new List<string>();
                foreach (string key in payload.FormValues)
                {
                    string[] values = payload.FormValues.GetValues(key);
                    if (values == null)
                    {
                        continue;
                    }

                    foreach (string value in values)
                    {
                        entries.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)));
                    }
                }

                return string.Join("&", entries);
            }

            return null;
        }

        /// <inheritdoc />
        public string GetHeader(string name, int index = -1)
        {
            string value = CreateKnownRequestHeaderValue(index);
            if (value == null && payload.Headers != null)
            {
                value = payload.Headers[name];
            }

            return value;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetHeaderNames()
        {
            return payload.Headers.Keys.Cast<string>();
        }

        /// <inheritdoc />
        public string GetMethod()
        {
            return payload.Method;
        }

        /// <inheritdoc />
        public string GetProtocol()
        {
            return payload.Protocol;
        }

        /// <inheritdoc />
        public string GetUrl(string rawUrl)
        {
            return rawUrl;
        }

        private string CreateContentTypeHeader()
        {
            bool isDeletePostOrPut = string.Equals(payload.Method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(payload.Method, "POST", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(payload.Method, "PUT", StringComparison.OrdinalIgnoreCase);

            // override "Content-Type" header for DELETE, POST and PUT requests, otherwise ASP.NET won't 
            // read the Form collection.
            if (isDeletePostOrPut && payload.FormValues.Count > 0)
            {
                return "application/x-www-form-urlencoded";
            }

            return null;
        }

        private string CreateCookieHeader()
        {
            if ((payload.Cookies == null) || (payload.Cookies.Count == 0))
            {
                return null;
            }

            var cookies = new StringBuilder();
            foreach (string cookieName in payload.Cookies)
            {
                var cookie = payload.Cookies[cookieName];
                if (cookie == null)
                {
                    continue;
                }

                cookies.AppendFormat("{0}={1};", cookieName, cookie.Value);
            }

            return cookies.ToString();
        }

        private string CreateKnownRequestHeaderValue(int index)
        {
            switch (index)
            {
                case HttpWorkerRequest.HeaderContentType:
                    return CreateContentTypeHeader();

                case HttpWorkerRequest.HeaderCookie:
                    return CreateCookieHeader();

                default:
                    return null;
            }
        }
    }
}