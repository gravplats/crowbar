using System;
using System.Collections;
using System.Security.Principal;
using System.Web;

namespace Crowbar.Views
{
    internal class HttpContextStub : HttpContextBase
    {
        private readonly ViewSettings settings;

        private readonly HttpRequestBase request;
        private readonly HttpResponseBase response;

        private IDictionary items;

        public HttpContextStub(HttpResponse httpResponse, ViewSettings settings)
        {
            this.settings = settings;

            request = new HttpRequestStub(new HttpRequest("", "http://www.example.com", ""), settings);
            response = new HttpResponseStub(httpResponse);
        }

        public override HttpRequestBase Request
        {
            get { return request; }
        }

        public override HttpResponseBase Response
        {
            get { return response; }
        }

        public override IDictionary Items
        {
            get { return items ?? (items = new Hashtable()); }
        }

        public override IPrincipal User
        {
            get { return settings.User ?? base.User; }
            set { throw new NotSupportedException(); }
        }
    }
}