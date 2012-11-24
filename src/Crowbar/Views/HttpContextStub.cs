using System.Collections;
using System.Web;

namespace Crowbar.Views
{
    internal class HttpContextStub : HttpContextBase
    {
        private readonly HttpRequestBase request;
        private readonly HttpResponseBase response;

        private IDictionary items;

        public HttpContextStub(HttpResponse httpResponse, ViewSettings settings)
        {
            request = new HttpRequestStub(new HttpRequest("", "http://www.example.com", ""), settings);
            response = new HttpResponseWrapper(httpResponse);
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
    }
}