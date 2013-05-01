using System.Collections.Specialized;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Defines the HTTP request payload.
    /// </summary>
    public interface IHttpPayload
    {
        /// <summary>
        /// Gets or sets cookies.
        /// </summary>
        HttpCookieCollection Cookies { get; set; }

        /// <summary>
        /// Gets or sets the form values.
        /// </summary>
        NameValueCollection FormValues { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        NameValueCollection Headers { get; set; }

        /// <summary>
        /// Gets or set the HTTP method.
        /// </summary>
        string Method { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        string RequestBody { get; set; }

        /// <summary>
        /// Gets or sets the query string.
        /// </summary>
        string QueryString { get; set; }
    }
}