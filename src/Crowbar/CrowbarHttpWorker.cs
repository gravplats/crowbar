using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Linq;

namespace Crowbar
{
    internal class CrowbarHttpWorker : SimpleWorkerRequest
    {
        private readonly CrowbarResponse response;
        private readonly string bodyString;
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection formValues;
        private readonly NameValueCollection headers;
        private readonly string method;
        private readonly string protocol;
        private readonly IRequestWaitHandle handle;
        private readonly RawHttpRequest rawHttpRequest;

        public CrowbarHttpWorker(string path, ICrowbarHttpWorkerContext context, TextWriter output, CrowbarResponse response, IRequestWaitHandle handle)

            : base(path, context.QueryString, output)
        {
            bodyString = context.BodyString;
            cookies = context.Cookies;
            formValues = context.FormValues;
            headers = context.Headers;
            method = context.Method;
            protocol = context.Protocol;
            this.response = response;
            this.handle = handle;
            rawHttpRequest = new RawHttpRequest(method, protocol);
        }

        public override void EndOfRequest()
        {
            handle.Signal();
        }

        public string GetRawHttpRequest()
        {
            return rawHttpRequest.ToString();
        }

        public override string GetRawUrl()
        {
            string value = base.GetRawUrl();
            rawHttpRequest.SetPath(value);

            return value;
        }

        public override string GetProtocol()
        {
            return protocol;
        }

        public override string GetHttpVerbName()
        {
            return method;
        }

        public override string GetKnownRequestHeader(int index)
        {
            string name = GetKnownRequestHeaderName(index);
            string value = HandleKnownRequestHeader(index);

            if (value == null && headers != null)
            {
                value = headers[name];
            }

            rawHttpRequest.AddHeader(name, value);

            return value;
        }

        private string HandleKnownRequestHeader(int index)
        {
            switch (index)
            {
                case HeaderContentType:

                    bool isDeletePostOrPut = string.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase);

                    //Override "Content-Type" header for DELETE, POST and PUT requests, otherwise ASP.NET won't read the Form collection.
                    if (index == HeaderContentType && isDeletePostOrPut && formValues.Count > 0)
                    {
                        return "application/x-www-form-urlencoded";
                    }

                    return null;

                case HeaderCookie:
                    return MakeCookieHeader();

                default:
                    return null;

            }
        }

        private string MakeCookieHeader()
        {
            if ((cookies == null) || (cookies.Count == 0))
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (string cookieName in cookies)
            {
                sb.AppendFormat("{0}={1};", cookieName, cookies[cookieName].Value);
            }

            return sb.ToString();
        }

        public override string GetUnknownRequestHeader(string name)
        {
            if (headers == null)
            {
                return null;
            }

            string value = headers[name];
            rawHttpRequest.AddHeader(name, value);

            return value;
        }

        public override string[][] GetUnknownRequestHeaders()
        {
            if (headers == null)
            {
                return null;
            }

            var unknownHeaders = from key in headers.Keys.Cast<string>()
                                 let knownRequestHeaderIndex = GetKnownRequestHeaderIndex(key)
                                 where knownRequestHeaderIndex < 0
                                 select new[] { key, headers[key] };

            foreach (string[] unknownHeader in unknownHeaders)
            {
                if (unknownHeader.Length != 2)
                {
                    continue;
                }

                rawHttpRequest.AddHeader(unknownHeader[0], unknownHeader[1]);
            }

            return unknownHeaders.ToArray();
        }

        public override byte[] GetPreloadedEntityBody()
        {
            string body = HandlePreloadedEntityBody();
            if (string.IsNullOrWhiteSpace(body))
            {
                return base.GetPreloadedEntityBody();
            }

            rawHttpRequest.SetBody(body);
            return Encoding.UTF8.GetBytes(body);
        }

        private string HandlePreloadedEntityBody()
        {
            if (!string.IsNullOrEmpty(bodyString))
            {
                return bodyString;
            }

            if (formValues.Count > 0)
            {
                var entries = new List<string>();
                foreach (string key in formValues)
                {
                    string[] values = formValues.GetValues(key);
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

        public override bool IsSecure()
        {
            return string.Equals(protocol, "https", StringComparison.OrdinalIgnoreCase);
        }

        public override void SendCalculatedContentLength(int contentLength)
        {
            response.AddHeader("Content-Length", contentLength.ToString());
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
            string name = GetKnownResponseHeaderName(index);
            response.AddHeader(name, value);
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
            response.StatusCode = (HttpStatusCode)statusCode;
            response.StatusDescription = statusDescription;
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
            response.AddHeader(name, value);
        }
    }
}