using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crowbar.Mvc.Core;
using Raven.Client;

namespace Crowbar.Mvc
{
    public class CrowbarHttpApplication : HttpApplication, ICrowbarHttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public void SetDocumentStore(IDocumentStore store)
        {
            CrowbarControllerBase.Store = store;
        }
    }
}