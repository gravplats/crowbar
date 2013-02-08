using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Helpers;
using System.Xml.Linq;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Defines the response that a <see cref="Browser"/> request.
    /// </summary>
    public class BrowserResponse
    {
        /// <summary>
        /// Gets various context objects collected during the request.
        /// </summary>
        public AdvancedBrowserResponse Advanced { get; internal set; }

        /// <summary>
        /// Gets the Content Type of the HTTP response.
        /// </summary>
        public string ContentType
        {
            get
            {
                if (HttpResponse == null)
                {
                    throw new InvalidOperationException("The HTTP response object is null.");
                }

                return HttpResponse.ContentType;
            }
        }

        /// <summary>
        /// Gets the 'faked' Headers of the HTTP response.
        /// </summary>
        public NameValueCollection Headers { get; internal set; }

        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        public HttpResponse HttpResponse { get; internal set; }

        /// <summary>
        /// Gets the HTTP response body.
        /// </summary>
        public string ResponseBody { get; internal set; }

        /// <summary>
        /// Gets the raw HTTP request that was sent to the server.
        /// </summary>
        public string RawHttpRequest { get; internal set; }

        /// <summary>
        /// Gets the HTTP Status Code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                // When no route is found the response object is null. Are there any other cases when this is also true?
                if (HttpResponse == null)
                {
                    return HttpStatusCode.NotFound;
                }

                return (HttpStatusCode)HttpResponse.StatusCode;
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

        /// <summary>
        /// Returns an XElement of the XML in the response body.
        /// </summary>
        /// <returns>An XElement.</returns>
        public XElement AsXml()
        {
            return XElement.Parse(ResponseBody);
        }
    }
}