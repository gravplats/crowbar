using System.Collections.Generic;
using System.IO;

namespace Crowbar
{
    /// <summary>
    /// Provides funtionality for capturing the raw HTTP request.
    /// </summary>
    public class CrowbarHttpRequestCapture : ICrowbarHttpRequest
    {
        private readonly Dictionary<string, string> headers = new Dictionary<string, string>();

        private string body;
        private string method;
        private string url;
        private string protocol;

        private readonly ICrowbarHttpRequest request;

        /// <summary>
        /// Creates an instance of <see cref="CrowbarHttpRequestCapture"/>.
        /// </summary>
        /// <param name="request">The request.</param>
        public CrowbarHttpRequestCapture(ICrowbarHttpRequest request)
        {
            this.request = request;
        }

        /// <inheritdoc />
        public string Path
        {
            get { return request.Path; }
        }

        /// <inheritdoc />
        public string QueryString
        {
            get { return request.QueryString; }
        }

        /// <inheritdoc />
        public string GetRequestBody()
        {
            return body = request.GetRequestBody();
        }

        /// <inheritdoc />
        public string GetHeader(string name, int index)
        {
            string value = request.GetHeader(name, index);
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            // could be called multiple times with the same name during a simulated HTTP request.
            if (!headers.ContainsKey(name))
            {
                headers.Add(name, value);
            }

            return value;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetHeaderNames()
        {
            return request.GetHeaderNames();
        }

        /// <inheritdoc />
        public string GetMethod()
        {
            return method = request.GetMethod();
        }

        /// <inheritdoc />
        public string GetProtocol()
        {
            return protocol = request.GetProtocol();
        }

        /// <inheritdoc />
        public string GetUrl(string rawUrl)
        {
            return url = request.GetUrl(rawUrl);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine("{0} {1}://localhost{2}", method.ToUpper(), protocol, url);
                foreach (var header in headers)
                {
                    writer.WriteLine("{0}: {1}", header.Key, header.Value);
                }

                if (!string.IsNullOrWhiteSpace(body))
                {
                    writer.WriteLine();
                    writer.WriteLine(body);
                }

                return writer.ToString();
            }
        }
    }
}