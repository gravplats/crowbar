using System;

namespace Crowbar
{
    /// <summary>
    /// Defines common browser contexts.
    /// </summary>
    public static class As
    {
        /// <summary>
        /// An AJAX request context.
        /// </summary>
        public static Action<BrowserContext> AjaxRequest = ctx => ctx.AjaxRequest();
    }
}