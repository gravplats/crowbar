using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Linq;

namespace Crowbar
{
    internal class CrowbarHttpWorker : SimpleWorkerRequest
    {
        private readonly CrowbarRequest request;
        private readonly CrowbarResponse response;
        private readonly IRequestWaitHandle handle;

        private readonly RawHttpRequest rawHttpRequest;

        public CrowbarHttpWorker(CrowbarRequest request, CrowbarResponse response, IRequestWaitHandle handle)
            : base(request.Path, request.QueryString, response.Output)
        {
            this.request = request;
            this.response = response;
            this.handle = handle;

            rawHttpRequest = new RawHttpRequest(request.Method, request.Protocol);
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
            return request.Protocol;
        }

        public override string GetHttpVerbName()
        {
            return request.Method;
        }

        public override string GetKnownRequestHeader(int index)
        {
            string name = GetKnownRequestHeaderName(index);
            string value = HandleKnownRequestHeader(index);

            if (value == null && request.Headers != null)
            {
                value = request.Headers[name];
            }

            rawHttpRequest.AddHeader(name, value);

            return value;
        }

        private string HandleKnownRequestHeader(int index)
        {
            switch (index)
            {
                case HeaderContentType:

                    bool isDeletePostOrPut = string.Equals(request.Method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(request.Method, "POST", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(request.Method, "PUT", StringComparison.OrdinalIgnoreCase);

                    //Override "Content-Type" header for DELETE, POST and PUT requests, otherwise ASP.NET won't read the Form collection.
                    if (index == HeaderContentType && isDeletePostOrPut && request.FormValues.Count > 0)
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
            if ((request.Cookies == null) || (request.Cookies.Count == 0))
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (string cookieName in request.Cookies)
            {
                sb.AppendFormat("{0}={1};", cookieName, request.Cookies[cookieName].Value);
            }

            return sb.ToString();
        }

        public override string GetUnknownRequestHeader(string name)
        {
            if (request.Headers == null)
            {
                return null;
            }

            string value = request.Headers[name];
            rawHttpRequest.AddHeader(name, value);

            return value;
        }

        public override string[][] GetUnknownRequestHeaders()
        {
            if (request.Headers == null)
            {
                return null;
            }

            var unknownHeaders = from key in request.Headers.Keys.Cast<string>()
                                 let knownRequestHeaderIndex = GetKnownRequestHeaderIndex(key)
                                 where knownRequestHeaderIndex < 0
                                 select new[] { key, request.Headers[key] };

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

        public override bool IsSecure()
        {
            return string.Equals(request.Protocol, "https", StringComparison.OrdinalIgnoreCase);
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