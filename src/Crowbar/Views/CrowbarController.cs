using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Crowbar.Views
{
    internal class CrowbarController : Controller
    {
        public static string ToString(PartialViewContext partialViewContext, object viewModel, out HttpCookieCollection cookies)
        {
            if (partialViewContext == null)
            {
                throw new ArgumentException("partialViewContext");
            }

            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            string viewName = partialViewContext.ViewName;

            using (var writer = new StringWriter())
            {
                var httpRequest = new HttpRequest("", "http://www.example.com", "");
                var httpResponse = new HttpResponse(writer);

                // There are still dependencies on HttpContext.Currrent in the ASP.NET (MVC) framework, eg., AntiForgeryRequestToken (as of ASP.NET MVC 4).
                var httpContext = new HttpContext(httpRequest, httpResponse) { User = partialViewContext.User };
                System.Web.HttpContext.Current = httpContext;

                var controllerContext = CreateControllerContext(httpContext);

                var viewEngineResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                if (viewEngineResult == null)
                {
                    string message = "The partial view was not found.";
                    throw new ArgumentException(message, viewName);
                }

                var view = viewEngineResult.View;
                if (view == null)
                {
                    var locations = new StringBuilder();
                    foreach (string searchedLocation in viewEngineResult.SearchedLocations)
                    {
                        locations.AppendLine();
                        locations.Append(searchedLocation);
                    }

                    throw new ArgumentException("The partial view was not found. The following locations were searched: " + locations, viewName);
                }

                try
                {
                    var viewData = new ViewDataDictionary(viewModel);
                    var tempData = new TempDataDictionary();

                    var viewContext = new ViewContextStub(controllerContext, view, viewData, tempData, writer)
                    {
                        ClientValidationEnabled = partialViewContext.ClientValidationEnabled,
                        UnobtrusiveJavaScriptEnabled = partialViewContext.UnobtrusiveJavaScriptEnabled
                    };

                    view.Render(viewContext, httpResponse.Output);
                    cookies = controllerContext.HttpContext.Response.Cookies;

                    httpResponse.Flush();
                }
                finally
                {
                    viewEngineResult.ViewEngine.ReleaseView(controllerContext, view);
                }

                return writer.ToString();
            }
        }

        private static ControllerContext CreateControllerContext(HttpContext httpContext)
        {
            // The 'controller' route data value is required by VirtualPathProviderViewEngine.
            var routeData = new RouteData();
            routeData.Values["controller"] = typeof(CrowbarController).Name;

            var requestContext = new RequestContext(new HttpContextWrapper(httpContext), routeData);

            return new ControllerContext(requestContext, new CrowbarController());
        }
    }
}