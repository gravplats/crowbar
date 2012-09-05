using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Linq;

namespace Crowbar.Browsing
{
    internal class SimulatedWorkerRequest : SimpleWorkerRequest
    {
        private readonly string bodyString;
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection formValues;
        private readonly NameValueCollection headers;
        private readonly string method;
        private readonly string protocol;

        public SimulatedWorkerRequest(string path, ISimulatedWorkerRequestContext context, TextWriter output)
            : base(path, context.QueryString, output)
        {
            bodyString = context.BodyString;
            cookies = context.Cookies;
            formValues = context.FormValues;
            headers = context.Headers;
            method = context.Method;
            protocol = context.Protocol;
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
            //Override "Content-Type" header for DELETE, POST and PUT requests, otherwise ASP.NET won't read the Form collection.
            if (index == 12)
            {
                if (string.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase))
                {
                    if (formValues.Count > 0)
                    {
                        return "application/x-www-form-urlencoded";
                    }

                    return headers["Content-Type"];
                }
            }

            switch (index)
            {
                case 0x19:
                    return MakeCookieHeader();

                default:
                    if (headers == null)
                    {
                        return null;
                    }

                    return headers[GetKnownRequestHeaderName(index)];
            }
        }

        public override string GetUnknownRequestHeader(string name)
        {
            if (headers == null)
            {
                return null;
            }

            return headers[name];
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

            return unknownHeaders.ToArray();
        }

        public override byte[] GetPreloadedEntityBody()
        {
            if (!string.IsNullOrEmpty(bodyString))
            {
                return Encoding.UTF8.GetBytes(bodyString);
            }

            if (formValues.Count > 0)
            {
                var builder = new StringBuilder();
                foreach (string key in formValues)
                {
                    builder.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(formValues[key]));
                }

                return Encoding.UTF8.GetBytes(builder.ToString());
            }

            return base.GetPreloadedEntityBody();
        }

        public override bool IsSecure()
        {
            return string.Equals(protocol, "https", StringComparison.OrdinalIgnoreCase);
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
    }
}