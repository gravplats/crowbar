using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;

namespace Raven.Web
{
    public class RavenMvcApplication : HttpApplication
    {
        public class RavenRazorViewEngine : RazorViewEngine
        {
            public RavenRazorViewEngine() : this(null) { }

            public RavenRazorViewEngine(IViewPageActivator viewPageActivator)
                : base(viewPageActivator)
            {
                ViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml",
                };

                MasterLocationFormats = new[] {
                    "~/Views/{0}.cshtml",
                };

                PartialViewLocationFormats = new[] {
                    "~/Views/{0}.cshtml",
                };

                FileExtensions = new[] {
                    "cshtml"
                };
            }
        }

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

        private void RegisterRavenDb()
        {
            if (RunningUserStories)
            {
                return;
            }

            // Need to setup a RavenDB for this to work.
            var store = new DocumentStore { ConnectionStringName = "RavenDB" }.Initialize();
            SetDocumentStore(store);
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
            engines.Add(new RavenRazorViewEngine());
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
            RegisterViewEngines();
        }

        public void SetDocumentStore(IDocumentStore store)
        {
            RavenController.Store = store;
        }
    }
}