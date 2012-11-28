using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web
{
    public class CrowbarHttpApplication : HttpApplication
    {
        public class CrowbarRazorViewEngine : RazorViewEngine
        {
            public CrowbarRazorViewEngine() : this(null) { }

            public CrowbarRazorViewEngine(IViewPageActivator viewPageActivator)
                : base(viewPageActivator)
            {
                ViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml"
                };

                MasterLocationFormats = new[] {
                    "~/Views/{0}.cshtml"
                };

                PartialViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml"
                };

                FileExtensions = new[] {
                    "cshtml"
                };
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
            engines.Add(new CrowbarRazorViewEngine());
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
            RegisterViewEngines();
        }
    }
}