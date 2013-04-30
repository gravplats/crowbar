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
        private readonly ICrowbarRequest request;

        /// <summary>
        /// Creates an instance of <see cref="CrowbarHttpRequest"/>.
        /// <param name="path">The requested path.</param>
        /// <param name="request">The request.</param>
        /// </summary>
        public CrowbarHttpRequest(string path, ICrowbarRequest request)
        {
            Path = path;
            this.request = request;
        }

        /// <inheritdoc />
        public string Path { get; private set; }

        /// <inheritdoc />
        public string QueryString
        {
            get { return request.QueryString; }
        }

        /// <inheritdoc />
        public string GetRequestBody()
        {
            if (!string.IsNullOrEmpty(request.RequestBody))
            {
                return request.RequestBody;
            }

            if (request.FormValues.Count > 0)
            {
                var entries = new List<string>();
                foreach (string key in request.FormValues)
                {
                    string[] values = request.FormValues.GetValues(key);
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
            if (value == null && request.Headers != null)
            {
                value = request.Headers[name];
            }

            return value;
        }

        public IEnumerable<string> GetHeaderNames()
        {
            return request.Headers.Keys.Cast<string>();
        }

        /// <inheritdoc />
        public string GetMethod()
        {
            return request.Method;
        }

        /// <inheritdoc />
        public string GetProtocol()
        {
            return request.Protocol;
        }

        /// <inheritdoc />
        public string GetUrl(string rawUrl)
        {
            return rawUrl;
        }

        private string CreateContentTypeHeader()
        {
            bool isDeletePostOrPut = string.Equals(request.Method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(request.Method, "PUT", StringComparison.OrdinalIgnoreCase);

            // override "Content-Type" header for DELETE, POST and PUT requests, otherwise ASP.NET won't 
            // read the Form collection.
            if (isDeletePostOrPut && request.FormValues.Count > 0)
            {
                return "application/x-www-form-urlencoded";
            }

            return null;
        }

        private string CreateCookieHeader()
        {
            if ((request.Cookies == null) || (request.Cookies.Count == 0))
            {
                return null;
            }

            var cookies = new StringBuilder();
            foreach (string cookieName in request.Cookies)
            {
                var cookie = request.Cookies[cookieName];
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