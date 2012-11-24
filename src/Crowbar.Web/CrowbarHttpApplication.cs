using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Raven.Client;

namespace Crowbar.Web
{
    public class CrowbarHttpApplication : HttpApplication
    {
        private static void RegisterGlobalFilters()
        {
            var filters = GlobalFilters.Filters;
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterRoutes()
        {
            var routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapAttributeRoutes();
        }

        private static void RegisterViewEngines()
        {
            var engines = ViewEngines.Engines;
            engines.Clear();
            engines.Add(new RazorViewEngine());
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
            RegisterViewEngines();
        }

        public void SetDocumentStore(IDocumentStore store)
        {
            CrowbarControllerBase.Store = store;
        }
    }
}