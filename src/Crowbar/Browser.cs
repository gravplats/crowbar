using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Provides the capabilities of simulating an HTTP(S) request to an ASP.NET web application.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Performs a DELETE request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Delete(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("DELETE", path, context);
        }

        /// <summary>
        /// Performs a GET request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Get(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("GET", path, context);
        }

        /// <summary>
        /// Performs a POST request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Post(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("POST", path, context);
        }

        /// <summary>
        /// Performs a PUT request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Put(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("PUT", path, context);
        }

        /// <summary>
        /// Performs a request against the host application using the specified HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method that should be used.</param>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="initialize">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse PerformRequest(string method, string path, Action<BrowserContext> initialize = null)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            var context = new BrowserContext(method);
            if (initialize != null)
            {
                initialize(context);
            }

            path = path.RemoveLeadingSlash();
            path = context.ExtractQueryString(path);

            CrowbarContext.Reset();

            var output = new StringWriter();
            var workerRequest = new SimulatedWorkerRequest(path, context, output);

            HttpRuntime.ProcessRequest(workerRequest);

            return CreateResponse(output);
        }

        private static BrowserResponse CreateResponse(StringWriter output)
        {
            if (CrowbarContext.ExceptionContext != null)
            {
                var exception = CrowbarContext.ExceptionContext.Exception;
                if (exception.IsSerializable())
                {
                    throw new CrowbarException("The MVC application threw an exception.", exception);
                }

                throw new Exception(string.Format("The MVC application threw an exception: {0}", exception.Message));
            }

            var response = CrowbarContext.Response;
            if (response == null)
            {
                return new BrowserResponse();
            }

            // Cannot read headers from response as it is not supported, however it is possible to fake the 'Location' header.
            var headers = new NameValueCollection();
            if (!string.IsNullOrEmpty(response.RedirectLocation))
            {
                headers.Add("Location", response.RedirectLocation);
            }

            return new BrowserResponse
            {
                Advanced = new AdvancedBrowserResponse
                {
                    ActionExecutedContext = CrowbarContext.ActionExecutedContext,
                    ActionExecutingContext = CrowbarContext.ActionExecutingContext,
                    ResultExecutedContext = CrowbarContext.ResultExecutedContext,
                    ResultExecutingContext = CrowbarContext.ResultExecutingContext,
                    HttpSessionState = CrowbarContext.HttpSessionState
                },
                Headers = headers,
                ResponseBody = output.ToString(),
                Response = response,
            };
        }
    }
}