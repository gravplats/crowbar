using System;

namespace Crowbar
{
    /// <summary>
    /// Defines common HTTP payloads.
    /// </summary>
    public static class As
    {
        /// <summary>
        /// An AJAX request context.
        /// </summary>
        public static Action<HttpPayload> AjaxRequest = payload => payload.AjaxRequest();
    }
}