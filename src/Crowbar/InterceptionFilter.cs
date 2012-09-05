using System.Web.Mvc;

namespace Crowbar
{
    internal class InterceptionFilter : IActionFilter, IExceptionFilter, IResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null || CrowbarContext.ActionExecutingContext != null)
            {
                return;
            }

            var clone = Clone<ActionExecutingContext>(context);
            clone.ActionDescriptor = context.ActionDescriptor;
            clone.ActionParameters = context.ActionParameters;
            clone.Result = context.Result;

            CrowbarContext.ActionExecutingContext = clone;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context == null || CrowbarContext.ActionExecutedContext != null)
            {
                return;
            }

            var clone = Clone<ActionExecutedContext>(context);
            clone.ActionDescriptor = context.ActionDescriptor;
            clone.Canceled = context.Canceled;
            clone.Exception = context.Exception;
            clone.ExceptionHandled = context.ExceptionHandled;
            clone.Result = context.Result;

            CrowbarContext.ActionExecutedContext = clone;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context == null || CrowbarContext.ResultExecutingContext != null)
            {
                return;
            }

            var clone = Clone<ResultExecutingContext>(context);
            clone.Cancel = context.Cancel;
            clone.Result = context.Result;

            CrowbarContext.ResultExecutingContext = clone;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context == null || CrowbarContext.ResultExecutedContext != null)
            {
                return;
            }

            var clone = Clone<ResultExecutedContext>(context);
            clone.Canceled = context.Canceled;
            clone.Exception = context.Exception;
            clone.ExceptionHandled = context.ExceptionHandled;
            clone.Result = context.Result;

            CrowbarContext.ResultExecutedContext = clone;
        }

        public void OnException(ExceptionContext context)
        {
            if (context == null || CrowbarContext.ExceptionContext != null)
            {
                return;
            }

            var clone = Clone<ExceptionContext>(context);
            clone.Exception = context.Exception;
            clone.ExceptionHandled = context.ExceptionHandled;
            clone.Result = context.Result;

            CrowbarContext.ExceptionContext = clone;
        }

        private static T Clone<T>(ControllerContext context)
            where T : ControllerContext, new()
        {
            // Clone to get a more stable snapshot.
            return new T
            {
                Controller = context.Controller,
                HttpContext = context.HttpContext,
                RequestContext = context.RequestContext,
                RouteData = context.RouteData
            };
        }
    }
}