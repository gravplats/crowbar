using System;
using System.Text;
using System.Web.Hosting;
using System.Linq;

namespace Crowbar
{
    internal class CrowbarHttpWorker : SimpleWorkerRequest
    {
        private readonly ICrowbarHttpRequest request;
        private readonly CrowbarHttpResponse response;
        private readonly IRequestWaitHandle handle;

        public CrowbarHttpWorker(ICrowbarHttpRequest request, CrowbarHttpResponse response, IRequestWaitHandle handle)
            : base(request.Path, request.QueryString, response.Output)
        {
            this.request = request;
            this.response = response;
            this.handle = handle;
        }

        public override void EndOfRequest()
        {
            handle.Signal();
        }

        public override string GetRawUrl()
        {
            string url = base.GetRawUrl();
            return request.GetUrl(url);
        }

        public override string GetProtocol()
        {
            return request.GetProtocol();
        }

        public override string GetHttpVerbName()
        {
            return request.GetMethod();
        }

        public override string GetKnownRequestHeader(int index)
        {
            string name = GetKnownRequestHeaderName(index);
            return request.GetHeader(name, index);
        }

        public override string GetUnknownRequestHeader(string name)
        {
            return request.GetHeader(name);
        }

        public override string[][] GetUnknownRequestHeaders()
        {
            var unknownHeaders = from name in request.GetHeaderNames()
                                 let knownRequestHeaderIndex = GetKnownRequestHeaderIndex(name)
                                 where knownRequestHeaderIndex < 0
                                 select new[] { name, request.GetHeader(name, knownRequestHeaderIndex) };

            return unknownHeaders.ToArray();
        }

        public override byte[] GetPreloadedEntityBody()
        {
            string body = request.GetRequestBody();
            if (string.IsNullOrWhiteSpace(body))
            {
                return base.GetPreloadedEntityBody();
            }

            return Encoding.UTF8.GetBytes(body);
        }

        public override bool IsSecure()
        {
            return string.Equals(request.GetProtocol(), "https", StringComparison.OrdinalIgnoreCase);
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