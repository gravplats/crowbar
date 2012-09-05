using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Raven.Client;

namespace Crowbar
{
    public class ServerContext
    {
        public ServerContext(IDocumentStore store)
        {
            Cookies = new HttpCookieCollection();
            Store = store;
        }

        public HttpCookieCollection Cookies { get; private set; }

        public HttpSessionState Session { get; private set; }

        public IDocumentStore Store { get; private set; }

        public RequestResult Delete(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("DELETE", path, initialize);
        }

        public RequestResult Get(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("GET", path, initialize);
        }

        public RequestResult Post(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("POST", path, initialize);
        }

        public RequestResult Put(string path, Action<BrowserContext> initialize = null)
        {
            return PerformRequest("PUT", path, initialize);
        }

        public RequestResult PerformRequest(string method, string path, Action<BrowserContext> initialize = null)
        {
            var context = new BrowserContext(method);

            initialize = initialize ?? (ctx => { });
            initialize(context);

            return ProcessRequest(path, context);
        }

        private RequestResult ProcessRequest(string path, ISimulatedWorkerRequestContext context)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            path = path.RemoveLeadingSlash();

            // Parse out the querystring if provided.
            string query = "";
            int querySeparatorIndex = path.IndexOf("?");
            if (querySeparatorIndex >= 0)
            {
                query = path.Substring(querySeparatorIndex + 1);
                path = path.Substring(0, querySeparatorIndex);
            }

            context.QueryString = query;

            // Perform the request.
            LastRequestData.Reset();

            var output = new StringWriter();

            var workerRequest = new SimulatedWorkerRequest(path, context, output);
            HttpRuntime.ProcessRequest(workerRequest);

            // Capture the output.
            AddAnyNewCookiesToCookieCollection();
            Session = LastRequestData.HttpSessionState;

            return new RequestResult
            {
                ResponseText = output.ToString(),
                ActionExecutedContext = LastRequestData.ActionExecutedContext,
                ResultExecutedContext = LastRequestData.ResultExecutedContext,
                Response = LastRequestData.Response,
            };
        }

        private void AddAnyNewCookiesToCookieCollection()
        {
            if (LastRequestData.Response == null)
            {
                return;
            }

            var lastResponseCookies = LastRequestData.Response.Cookies;
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