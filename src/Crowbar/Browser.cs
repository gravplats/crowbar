using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Crowbar
{
    public class Browser
    {
        public Browser()
        {
            Cookies = new HttpCookieCollection();
        }

        public HttpCookieCollection Cookies { get; private set; }

        public HttpSessionState Session { get; private set; }

        public BrowserResponse Delete(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("DELETE", path, initialize);
        }

        public BrowserResponse Get(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("GET", path, initialize);
        }

        public BrowserResponse Post(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("POST", path, initialize);
        }

        public BrowserResponse Put(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("PUT", path, initialize);
        }

        public BrowserResponse PerformRequest(string method, string path, Action<BrowserContext> initialize = null)
        {
            var context = new BrowserContext(method);

            initialize = initialize ?? (ctx => { });
            initialize(context);

            return ProcessRequest(path, context);
        }

        private BrowserResponse ProcessRequest(string path, BrowserContext context)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            path = path.RemoveLeadingSlash();
            path = context.ExtractQueryString(path);

            // Perform the request.
            CrowbarContext.Reset();

            var output = new StringWriter();

            var workerRequest = new SimulatedWorkerRequest(path, context, output);
            HttpRuntime.ProcessRequest(workerRequest);

            // Capture the output.
            AddAnyNewCookiesToCookieCollection();
            Session = CrowbarContext.HttpSessionState;

            return CreateResponse(output);
        }

        private static BrowserResponse CreateResponse(StringWriter output)
        {
            if (CrowbarContext.ExceptionContext != null)
            {
                throw new Exception("The server throw an exception.", CrowbarContext.ExceptionContext.Exception);
            }

            // When no route is found the response object is null. Are there any other cases when this is also true?
            var response = CrowbarContext.Response;
            if (response == null)
            {
                return new BrowserResponse
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            // Cannot read headers from response as it is not supported, however it is possible to fake the 'Location' header.
            var headers = new NameValueCollection();
            if (!string.IsNullOrEmpty(response.RedirectLocation))
            {
                headers.Add("Location", response.RedirectLocation);
            }

            return new BrowserResponse
            {
                ActionExecutedContext = CrowbarContext.ActionExecutedContext,
                Headers = headers,
                ResponseText = output.ToString(),
                ResultExecutedContext = CrowbarContext.ResultExecutedContext,
                Response = response,
                StatusCode = (HttpStatusCode)response.StatusCode
            };
        }

        private void AddAnyNewCookiesToCookieCollection()
        {
            if (CrowbarContext.Response == null)
            {
                return;
            }

            var lastResponseCookies = CrowbarContext.Response.Cookies;
            if (lastResponseCookies == null)
            {
                return;
            }

            foreach (string cookieName in lastResponseCookies)
            {
                var cookie = lastResponseCookies[cookieName];
                if (Cookies[cookieName] != null)
                {
                    Cookies.Remove(cookieName);
                }

                if ((cookie.Expires == default(DateTime)) || (cookie.Expires > DateTime.Now))
                {
                    Cookies.Add(cookie);
                }
            }
        }
    }
}