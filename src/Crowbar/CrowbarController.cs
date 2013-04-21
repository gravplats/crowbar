using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Crowbar
{
    /// <summary>
    /// Dummy controller for rendering a partial view to a string.
    /// </summary>
    public class CrowbarController : Controller
    {
        /// <summary>
        /// Renders a partial view to a string in the specified context.
        /// </summary>
        /// <param name="crowbarViewContext">The view context.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="cookies">Any cookies that were captures as part of the response.</param>
        /// <returns>The view rendered as a string.</returns>
        public static string ToString(CrowbarViewContext crowbarViewContext, object viewModel, out HttpCookieCollection cookies)
        {
            string viewName = crowbarViewContext.ViewName;

            using (var writer = new StringWriter())
            {
                var httpRequest = new HttpRequest("", "http://www.example.com", "");
                var httpResponse = new HttpResponse(writer);

                var controllerContext = CreateControllerContext(httpRequest, httpResponse, crowbarViewContext);
                var viewEngineResult = crowbarViewContext.FindViewEngineResult(controllerContext);

                var view = viewEngineResult.View;
                if (view == null)
                {
                    var locations = new StringBuilder();
                    foreach (string searchedLocation in viewEngineResult.SearchedLocations)
                    {
                        locations.AppendLine();
                        locations.Append(searchedLocation);
                    }

                    throw new ArgumentException("The view was not found. The following locations were searched: " + locations, viewName);
                }

                try
                {
                    var viewData = new ViewDataDictionary(viewModel);
                    var tempData = new TempDataDictionary();

                    var viewContext = new ViewContextStub(controllerContext, view, viewData, tempData, writer)
                    {
                        ClientValidationEnabled = crowbarViewContext.ClientValidationEnabled,
                        UnobtrusiveJavaScriptEnabled = crowbarViewContext.UnobtrusiveJavaScriptEnabled
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

        private static ControllerContext CreateControllerContext(HttpRequest httpRequest, HttpResponse httpResponse, CrowbarViewContext crowbarViewContext)
        {
            // There are still dependencies on HttpContext.Currrent in the ASP.NET (MVC) framework, eg., AntiForgeryRequestToken (as of ASP.NET MVC 4).
            var httpContext = new HttpContext(httpRequest, httpResponse) { User = crowbarViewContext.User };
            System.Web.HttpContext.Current = httpContext;

            var requestContext = new RequestContext(new HttpContextWrapper(httpContext), crowbarViewContext.GetRouteData());
            return new ControllerContext(requestContext, new CrowbarController());
        }
    }
}