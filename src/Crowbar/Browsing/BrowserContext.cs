using System.Collections.Specialized;
using System.Web;

namespace Crowbar.Browsing
{
    public class BrowserContext : ISimulatedWorkerRequestContext
    {
        string ISimulatedWorkerRequestContext.BodyString { get; set; }

        HttpCookieCollection ISimulatedWorkerRequestContext.Cookies { get; set; }

        NameValueCollection ISimulatedWorkerRequestContext.FormValues { get; set; }

        NameValueCollection ISimulatedWorkerRequestContext.Headers { get; set; }

        string ISimulatedWorkerRequestContext.Method { get; set; }

        string ISimulatedWorkerRequestContext.Protocol { get; set; }

        string ISimulatedWorkerRequestContext.QueryString { get; set; }
    }
}