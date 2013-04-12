using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Crowbar.Demo.Mvc.Async.Application.Infrastructure;
using Ninject;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Crowbar.Demo.Mvc.Async.Application
{
    public class App : HttpApplication
    {
        public IKernel Kernel
        {
            get
            {
                var kernel = DependencyResolver.Current.GetService<IKernel>();
                if (kernel == null)
                {
                    throw new InvalidOperationException("Ninject kernel is null.");
                }

                return kernel;
            }
        }

        protected void Application_Start()
        {
            RegisterGlobalFilters();
            RegisterRoutes();
            RegisterViewEngines();
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
    }
}