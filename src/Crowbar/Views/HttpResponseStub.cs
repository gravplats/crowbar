using System.Web;

namespace Crowbar.Views
{
    internal class HttpResponseStub : HttpResponseWrapper
    {
        private readonly HttpCookieCollection cookies;

        public HttpResponseStub(HttpResponse httpResponse)
            : base(httpResponse)
        {
            // Need to override cookies because with do not want the collection to be associated with a response, it will
            // result in a NullReferenceException, because the response does not have an associated HTTP context.
            cookies = new HttpCookieCollection();
        }

        public override HttpCookieCollection Cookies
        {
            get { return cookies; }
        }
    }
}