using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace Crowbar
{
    public class BrowserResponse
    {
        public ActionExecutedContext ActionExecutedContext { get; set; }

        public NameValueCollection Headers { get; set; }
        
        public HttpResponse Response { get; set; }

        public string ResponseText { get; set; }

        public ResultExecutedContext ResultExecutedContext { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}