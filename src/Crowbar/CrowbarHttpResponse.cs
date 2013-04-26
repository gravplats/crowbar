using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    internal class CrowbarHttpResponse : HttpResponseWrapper
    {
        private readonly NameValueCollection responseHeaders;

        public CrowbarHttpResponse(HttpResponse httpResponse, NameValueCollection responseHeaders)
            : base(httpResponse)
        {
            this.responseHeaders = responseHeaders;
        }

        public override NameValueCollection Headers
        {
            get { return responseHeaders; }
        }
    }
}