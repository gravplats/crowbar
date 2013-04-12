using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure;
using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate;
using Ninject;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Crowbar.Demo.Mvc.NHibernate.Application
{
    public class App : HttpApplication, INHibernateSessionKey
    {
        public const string NHibernateSessionKey = "App.CurrentNHibernateSession";

        public App()
        {
            NHibernateSessionPerRequestAdapter.Register(this);
        }

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

        public string GetNHibernateSessionKey()
        {
            return NHibernateSessionKey;
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