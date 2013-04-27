using System.Collections.Specialized;
using System.Web;
using System.Web.Helpers;
using System.Xml.Linq;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Defines the client response.
    /// </summary>
    public class ClientResponse
    {
        /// <summary>
        /// Gets various context objects collected during the request.
        /// </summary>
        public AdvancedClientResponse Advanced { get; internal set; }

        /// <summary>
        /// Gets the Content Type of the HTTP response.
        /// </summary>
        public string ContentType
        {
            get { return Headers["Content-Type"]; }
        }

        /// <summary>
        /// Gets the headers of the HTTP response.
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
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets the status description.
        /// </summary>
        public string StatusDescription { get; internal set; }

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