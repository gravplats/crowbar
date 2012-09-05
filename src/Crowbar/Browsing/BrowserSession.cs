using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Crowbar.Interception;
using Raven.Client;

namespace Crowbar.Browsing
{
    public class BrowserSession
    {
        public BrowserSession(IDocumentStore store)
        {
            Cookies = new HttpCookieCollection();
            Store = store;
        }

        public HttpSessionState Session { get; private set; }
        public HttpCookieCollection Cookies { get; private set; }

        public IDocumentStore Store { get; private set; }


        public RequestResult Get(string url)
        {
            return ProcessRequest(url, HttpVerbs.Get, new NameValueCollection());
        }

        public RequestResult Post(string url, object formData)
        {
            var formNameValueCollection = NameValueCollectionConversions.ConvertFromObject(formData);
            return ProcessRequest(url, HttpVerbs.Post, formNameValueCollection);
        }

        private RequestResult ProcessRequest(string url, HttpVerbs httpVerb = HttpVerbs.Get, NameValueCollection formValues = null)
        {
            return ProcessRequest(url, httpVerb, formValues, null);
        }

        private RequestResult ProcessRequest(string url, HttpVerbs httpVerb, NameValueCollection formValues, NameValueCollection headers)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            url = url.RemoveLeadingSlash();

            // Parse out the querystring if provided.
            string query = "";
            int querySeparatorIndex = url.IndexOf("?");
            if (querySeparatorIndex >= 0)
            {
                query = url.Substring(querySeparatorIndex + 1);
                url = url.Substring(0, querySeparatorIndex);
            }

            // Perform the request.
            LastRequestData.Reset();

            var output = new StringWriter();
            string method = httpVerb.ToString().ToLower();

            ISimulatedWorkerRequestContext context = new BrowserContext();
            context.BodyString = "";
            context.Cookies = Cookies;
            context.FormValues = formValues;
            context.Headers = headers;
            context.Method = method;
            context.Protocol = "http";
            context.QueryString = query;

            var workerRequest = new SimulatedWorkerRequest(url, context, output);
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