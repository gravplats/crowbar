using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Provides capabilities of simulating an HTTP(S) request to an ASP.NET MVC application.
    /// </summary>
    public class Client
    {
        private readonly int mvcMajorVersion;
        private readonly IHttpPayloadDefaults defaults;

        /// <summary>
        /// Creates an instance of <see cref="Client"/>.
        /// </summary>
        /// <param name="mvcMajorVersion">The major version of the MVC framework.</param>
        /// <param name="defaults"></param>
        public Client(int mvcMajorVersion, IHttpPayloadDefaults defaults = null)
        {
            this.mvcMajorVersion = mvcMajorVersion;
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

            var payload = new HttpPayload(mvcMajorVersion, method);
            if (defaults != null)
            {
                defaults.ApplyTo(payload);
            }

            path = path.RemoveLeadingSlash();
            path = payload.ExtractQueryString(path);

            overrides.TryInvoke(payload);

            CrowbarContext.Reset();

            var output = new StringWriter();
            var workerRequest = new SimulatedWorkerRequest(path, payload, output);

            HttpRuntime.ProcessRequest(workerRequest);

            string rawHttpRequest = workerRequest.GetRawHttpRequest();
            return CreateResponse(output, rawHttpRequest);
        }

        /// <summary>
        /// Creates the client response.
        /// </summary>
        /// <param name="output">The writer to which the output should be written.</param>
        /// <param name="rawHttpRequest">The raw HTTP request.</param>
        /// <returns>The client response.</returns>
        protected virtual ClientResponse CreateResponse(StringWriter output, string rawHttpRequest)
        {
            if (CrowbarContext.ExceptionContext != null)
            {
                Throw(rawHttpRequest);
            }

            var response = CrowbarContext.HttpResponse;
            if (response == null)
            {
                return new ClientResponse();
            }

            // Cannot read headers from response as it is not supported, however it is possible to fake the 'Location' header.
            var headers = new NameValueCollection();
            if (!string.IsNullOrEmpty(response.RedirectLocation))
            {
                headers.Add("Location", response.RedirectLocation);
            }

            return new ClientResponse
            {
                Advanced = new AdvancedClientResponse
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