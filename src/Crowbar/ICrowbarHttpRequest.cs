using System.Collections.Generic;

namespace Crowbar
{
    /// <summary>
    /// Represents the request of an HTTP worker.
    /// </summary>
    public interface ICrowbarHttpRequest
    {
        /// <summary>
        /// Gets the requested path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        string QueryString { get; }

        /// <summary>
        /// Gets the request body.
        /// </summary>
        /// <returns>The request body.</returns>
        string GetRequestBody();

        /// <summary>
        /// Gets the value of header with the specified name.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="index">The header index.</param>
        /// <returns>The value of the header.</returns>
        string GetHeader(string name, int index = -1);

        /// <summary>
        /// Gets the header names.
        /// </summary>
        /// <returns>The header names.</returns>
        IEnumerable<string> GetHeaderNames();

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <returns>The HTTP method.</returns>
        string GetMethod();

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <returns>The protocol.</returns>
        string GetProtocol();

        /// <summary>
        /// Gets the requested URL.
        /// </summary>
        /// <param name="rawUrl">The raw URL.</param>
        /// <returns>The requested URL.</returns>
        string GetUrl(string rawUrl);
    }
}