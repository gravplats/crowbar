using System;
using System.Reflection;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Mvc.Core
{
    public class AjaxTestController : CrowbarControllerBase
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public sealed class AjaxOnlyAttribute : ActionMethodSelectorAttribute
        {
            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
            {
                if (controllerContext == null)
                {
                    throw new ArgumentNullException("controllerContext");
                }

                return (controllerContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
            }
        }

        [DELETE(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Delete()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [GET(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Get()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [POST(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Post()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [PUT(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Put()
        {
            return Assert(() => Request.IsAjaxRequest());
        }
    }
}