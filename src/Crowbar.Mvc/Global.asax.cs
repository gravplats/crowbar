using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Crowbar.Mvc.Core;
using Raven.Client;

namespace Crowbar.Mvc
{
    public class MvcApplication : HttpApplication, IRavenDbHttpApplication
    {
        public IDocumentStore Store
        {
            set { TestController.Store = value; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null, "{id}", new { controller = "Test", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}