using System;

namespace Crowbar
{
    internal static class As
    {
        public static Action<BrowserContext> AjaxRequest = ctx => ctx.AjaxRequest();
    }
}