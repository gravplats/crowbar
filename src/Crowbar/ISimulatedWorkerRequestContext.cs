using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    public interface ISimulatedWorkerRequestContext
    {
        string BodyString { get; set; }

        HttpCookieCollection Cookies { get; set; }

        NameValueCollection FormValues { get; set; }

        NameValueCollection Headers { get; set; }

        string Method { get; set; }

        string Protocol { get; set; }

        string QueryString { get; set; }
    }
}