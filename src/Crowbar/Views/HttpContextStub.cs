using System;
using System.Collections;
using System.Security.Principal;
using System.Web;

namespace Crowbar.Views
{
    internal class HttpContextStub : HttpContextBase
    {
        private readonly PartialViewContext partialViewContext;

        private readonly HttpRequestBase request;
        private readonly HttpResponseBase response;

        private IDictionary items;

        public HttpContextStub(HttpResponse httpResponse, PartialViewContext partialViewContext)
        {
            this.partialViewContext = partialViewContext;

            request = new HttpRequestStub(new HttpRequest("", "http://www.example.com", ""), partialViewContext);
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
            get { return partialViewContext.User ?? base.User; }
            set { throw new NotSupportedException(); }
        }
    }
}