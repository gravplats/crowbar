using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Raven.Client;

namespace Crowbar.Demo.Mvc.Raven.Application
{
    public class App : HttpApplication
    {
        private static bool RunningUserStories
        {
            get
            {
                string value = ConfigurationManager.AppSettings["RunningUserStories"];
                bool runningUserStories;

                return !string.IsNullOrEmpty(value) && bool.TryParse(value, out runningUserStories) && runningUserStories;
            }
        }

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
            engines.Add(new AppRazorViewEngine());
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
            RegisterViewEngines();
        }

        public void SetDocumentStore(IDocumentStore store)
        {
            AppController.Store = store;
        }
    }
}