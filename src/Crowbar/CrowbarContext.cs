using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Crowbar
{
    internal static class CrowbarContext
    {
        public static ActionExecutingContext ActionExecutingContext { get; set; }
        public static ActionExecutedContext ActionExecutedContext { get; set; }

        public static ResultExecutingContext ResultExecutingContext { get; set; }
        public static ResultExecutedContext ResultExecutedContext { get; set; }

        public static ExceptionContext ExceptionContext { get; set; }

        public static HttpSessionState HttpSessionState { get; set; }

        public static HttpResponse HttpResponse { get; set; }

        public static void Reset()
        {
            ActionExecutedContext = null;
            ActionExecutingContext = null;

            ResultExecutingContext = null;
            ResultExecutedContext = null;

            ExceptionContext = null;

            HttpSessionState = null;
            HttpResponse = null;
        }
    }
}