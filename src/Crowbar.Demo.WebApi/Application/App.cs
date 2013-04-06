using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Crowbar.Demo.WebApi.Application
{
    public class App : HttpApplication
    {
        protected void Application_Start()
        {
            RegisterApiRoutes();
            RegisterGlobalFilters();
        }

        private static void RegisterApiRoutes()
        {
            var config = GlobalConfiguration.Configuration;
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void RegisterGlobalFilters()
        {
            var filters = GlobalFilters.Filters;
            filters.Add(new HandleErrorAttribute());
        }
    }
}