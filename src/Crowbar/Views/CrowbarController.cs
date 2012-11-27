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
        private static ControllerContext CreateControllerContext(HttpResponse httpResponse, ViewSettings settings)
        {
            // The 'controller' route data value is required by VirtualPathProviderViewEngine.
            var routeData = new RouteData();
            routeData.Values["controller"] = typeof(CrowbarController).Name;

            var httpContext = new HttpContextStub(httpResponse, settings);
            var requestContext = new RequestContext(httpContext, routeData);
            return new ControllerContext(requestContext, new CrowbarController());
        }

        public static string ToString(PartialViewContext partialViewContext, object viewModel, out HttpCookieCollection cookies)
        {
            if (partialViewContext == null)
            {
                throw new ArgumentException("PartialViewContext");
            }

            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel");
            }

            string viewName = partialViewContext.ViewName;

            using (var writer = new StringWriter())
            {
                var viewSettings = partialViewContext.ViewSettings;

                var httpResponse = new HttpResponse(writer);
                var controllerContext = CreateControllerContext(httpResponse, viewSettings);

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
                        ClientValidationEnabled = viewSettings.ClientValidationEnabled,
                        UnobtrusiveJavaScriptEnabled = viewSettings.UnobtrusiveJavaScriptEnabled
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
    }
}