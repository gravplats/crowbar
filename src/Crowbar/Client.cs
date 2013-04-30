using System;
using System.IO;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Provides capabilities of simulating an HTTP(S) request to an ASP.NET MVC application.
    /// </summary>
    public class Client
    {
        private readonly IHttpPayloadDefaults defaults;

        /// <summary>
        /// Creates an instance of <see cref="Client"/>.
        /// </summary>
        /// <param name="defaults">Default HTTP payload settings, if any.</param>
        public Client(IHttpPayloadDefaults defaults = null)
        {
            this.defaults = defaults;
        }

        /// <summary>
        /// Performs a DELETE request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the HTTP payload for the request.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public virtual ClientResponse Delete(string path, Action<HttpPayload> context = null)
        {
            return PerformRequest("DELETE", path, context);
        }

        /// <summary>
        /// Performs a GET request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the HTTP payload for the request.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public virtual ClientResponse Get(string path, Action<HttpPayload> context = null)
        {
            return PerformRequest("GET", path, context);
        }

        /// <summary>
        /// Performs a POST request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the HTTP payload for the request.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public virtual ClientResponse Post(string path, Action<HttpPayload> context = null)
        {
            return PerformRequest("POST", path, context);
        }

        /// <summary>
        /// Performs a PUT request against the host application.
        /// </summary>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="context">A configuration object for providing the HTTP payload for the request.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public virtual ClientResponse Put(string path, Action<HttpPayload> context = null)
        {
            return PerformRequest("PUT", path, context);
        }

        /// <summary>
        /// Performs a request against the host application using the specified HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method that should be used.</param>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="overrides">A configuration object for providing the HTTP payload for the request.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public virtual ClientResponse PerformRequest(string method, string path, Action<HttpPayload> overrides = null)
        {
            Ensure.NotNull(method, "method");
            Ensure.NotNull(path, "path");

            var payload = new HttpPayload(method);
            if (defaults != null)
            {
                defaults.ApplyTo(payload);
            }

            path = path.RemoveLeadingSlash();
            path = payload.ExtractQueryString(path);

            overrides.TryInvoke(payload);

            CrowbarContext.Reset();

            var request = new CrowbarHttpRequestCapture(new CrowbarHttpRequest(path, payload));
            var response = new CrowbarHttpResponse();

            using (var handle = CreateRequestWaitHandle())
            {
                var worker = new CrowbarHttpWorker(request, response, handle);
                HttpRuntime.ProcessRequest(worker);
                handle.Wait();

                return CreateResponse(request, response);
            }
        }

        /// <summary>
        /// Creates the request wait handle which is used to signal the end of the request.
        /// </summary>
        /// <returns>The request wait handle.</returns>
        protected virtual IRequestWaitHandle CreateRequestWaitHandle()
        {
            return new RequestWaitHandle();
        }

        /// <summary>
        /// Creates the client response.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="response">The HTTP response.</param>
        /// <returns>The client response.</returns>
        protected virtual ClientResponse CreateResponse(ICrowbarHttpRequest request, CrowbarHttpResponse response)
        {
            if (CrowbarContext.ExceptionContext != null)
            {
                Throw(request);
            }

            return new ClientResponse
            {
                Advanced = new AdvancedClientResponse
                {
                    ActionExecutedContext = CrowbarContext.ActionExecutedContext,
                    ActionExecutingContext = CrowbarContext.ActionExecutingContext,
                    ResultExecutedContext = CrowbarContext.ResultExecutedContext,
                    ResultExecutingContext = CrowbarContext.ResultExecutingContext,
                    HttpResponse = CrowbarContext.HttpResponse,
                    HttpSessionState = CrowbarContext.HttpSessionState
                },
                Headers = response.GetHeaders(),
                ResponseBody = response.GetResponseBody(),
                RawHttpRequest = request.ToString(),
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription
            };
        }

        /// <summary>
        /// Throws an exception due to an error during the request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        protected virtual void Throw(ICrowbarHttpRequest request)
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine("The MVC application threw an exception during the following HTTP request:");
                writer.WriteLine();
                writer.WriteLine(request);

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