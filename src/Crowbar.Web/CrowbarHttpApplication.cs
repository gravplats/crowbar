using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Raven.Client;

namespace Crowbar.Web
{
    public class CrowbarHttpApplication : HttpApplication
    {
        public static void RegisterGlobalFilters()
        {
            var filters = GlobalFilters.Filters;
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes()
        {
            var routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapAttributeRoutes();
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
        }

        public void SetDocumentStore(IDocumentStore store)
        {
            CrowbarControllerBase.Store = store;
        }
    }
}