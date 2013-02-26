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
        private readonly int mvcMajorVersion;
        private readonly Action<BrowserContext> defaults;

        /// <summary>
        /// Creates an instance of <see cref="Browser"/>.
        /// </summary>
        /// <param name="mvcMajorVersion">The major version of the MVC framework.</param>
        /// <param name="defaults">The default browser context settings.</param>
        public Browser(int mvcMajorVersion, Action<BrowserContext> defaults)
        {
            this.mvcMajorVersion = mvcMajorVersion;
            this.defaults = defaults;
        }

        /// <summary>
        /// Performs a DELETE request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public virtual BrowserResponse Delete(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("DELETE", path, context);
        }

        /// <summary>
        /// Performs a GET request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public virtual BrowserResponse Get(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("GET", path, context);
        }

        /// <summary>
        /// Performs a POST request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public virtual BrowserResponse Post(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("POST", path, context);
        }

        /// <summary>
        /// Performs a PUT request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public virtual BrowserResponse Put(string path, Action<BrowserContext> context = null)
        {
            return PerformRequest("PUT", path, context);
        }

        /// <summary>
        /// Performs a request against the host application using the specified HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method that should be used.</param>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="overrides">A configuration object for providing the browser context for the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public virtual BrowserResponse PerformRequest(string method, string path, Action<BrowserContext> overrides = null)
        {
            Ensure.NotNull(method, "method");
            Ensure.NotNull(path, "path");

            var context = new BrowserContext(mvcMajorVersion, method);

            path = path.RemoveLeadingSlash();
            path = context.ExtractQueryString(path);

            defaults.TryInvoke(context);
            overrides.TryInvoke(context);

            CrowbarContext.Reset();

            var output = new StringWriter();
            var workerRequest = new SimulatedWorkerRequest(path, context, output);

            HttpRuntime.ProcessRequest(workerRequest);

            string rawHttpRequest = workerRequest.GetRawHttpRequest();
            return CreateResponse(output, rawHttpRequest);
        }

        /// <summary>
        /// Creates the browser response.
        /// </summary>
        /// <param name="output">The writer to which the output should be written.</param>
        /// <param name="rawHttpRequest">The raw HTTP request.</param>
        /// <returns>The browser response.</returns>
        protected virtual BrowserResponse CreateResponse(StringWriter output, string rawHttpRequest)
        {
            if (CrowbarContext.ExceptionContext != null)
            {
                Throw(rawHttpRequest);
            }

            var response = CrowbarContext.HttpResponse;
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
                HttpResponse = response,
                ResponseBody = output.ToString(),
                RawHttpRequest = rawHttpRequest
            };
        }

        /// <summary>
        /// Throws an exception due to an error during the request.
        /// </summary>
        /// <param name="rawHttpRequest">The raw HTTP request.</param>
        protected virtual void Throw(string rawHttpRequest)
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine("The MVC application threw an exception during the following HTTP request:");
                writer.WriteLine();
                writer.WriteLine(rawHttpRequest);

                var exception = CrowbarContext.ExceptionContext.Exception;
                if (exception.IsSerializable())
                {
                    throw new CrowbarException(writer.ToString(), exception);
                }

                writer.WriteLine(exception.Message);

                throw new Exception(writer.ToString());
            }
        }
    }
}