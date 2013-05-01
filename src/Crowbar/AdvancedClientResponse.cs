using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Crowbar
{
    /// <summary>
    /// Provides access to various state objects collected during the HTTP request cycle.
    /// </summary>
    public class AdvancedClientResponse
    {
        /// <summary>
        /// Gets the context for the OnActionExecuted method.
        /// </summary>
        public ActionExecutedContext ActionExecutedContext { get; internal set; }

        /// <summary>
        /// Gets the context for the OnActionExecuting method.
        /// </summary>
        public ActionExecutingContext ActionExecutingContext { get; internal set; }

        /// <summary>
        /// Gets the context for the OnResultExecuted method.
        /// </summary>
        public ResultExecutedContext ResultExecutedContext { get; internal set; }

        /// <summary>
        /// Gets the context for the OnResultExecuting method.
        /// </summary>
        public ResultExecutingContext ResultExecutingContext { get; internal set; }

        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        public HttpResponse HttpResponse { get; internal set; }

        /// <summary>
        /// Gets the HTTP session state.
        /// </summary>
        public HttpSessionState HttpSessionState { get; internal set; }
    }
}