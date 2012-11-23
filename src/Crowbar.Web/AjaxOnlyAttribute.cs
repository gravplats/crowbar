using System;
using System.Reflection;
using System.Web.Mvc;

namespace Crowbar.Web
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
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
}