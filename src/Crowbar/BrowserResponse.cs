using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Helpers;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Defines the response that a <see cref="Browser"/> request.
    /// </summary>
    public class BrowserResponse
    {
        /// <summary>
        /// Gets the Content Type of the HTTP response.
        /// </summary>
        public string ContentType
        {
            get
            {
                if (Response == null)
                {
                    throw new InvalidOperationException("The HTTP response object is null.");
                }

                return Response.ContentType;
            }
        }

        /// <summary>
        /// Gets or sets the 'faked' Headers of the HTTP response.
        /// </summary>
        public NameValueCollection Headers { get; set; }

        /// <summary>
        /// Gets or set the HTTP response.
        /// </summary>
        public HttpResponse Response { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response body.
        /// </summary>
        public string ResponseBody { get; set; }

        /// <summary>
        /// Gets the HTTP Status Code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                // When no route is found the response object is null. Are there any other cases when this is also true?
                if (Response == null)
                {
                    return HttpStatusCode.NotFound;
                }

                return (HttpStatusCode) Response.StatusCode;
            }
        }

        /// <summary>
        /// Returns a DOM representation of the HTML document in the response body.
        /// </summary>
        /// <returns>A DOM representation of the HTML document.</returns>
        public CQ AsCsQuery()
        {
            return CQ.Create(ResponseBody);
        }

        /// <summary>
        /// Gets a dynamic representation of the JSON in the response body.
        /// </summary>
        /// <returns>A dynamic representation of the HTTP response body.</returns>
        public dynamic AsJson()
        {
            return Json.Decode(ResponseBody);
        }
    }
}