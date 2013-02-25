using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Crowbar
{
    /// <summary>
    /// Provides functionality for creating an MVC application.
    /// </summary>
    public class MvcApplicationFactory
    {
        private static readonly MethodInfo getApplicationInstanceMethod;
        private static readonly MethodInfo recycleApplicationInstanceMethod;

        static MvcApplicationFactory()
        {
            // Get references to MethodInfo:s that we'll need to use later to bypass nonpublic access restrictions.
            var httpApplicationFactory = typeof(HttpContext).Assembly.GetType("System.Web.HttpApplicationFactory", true);
            getApplicationInstanceMethod = httpApplicationFactory.GetMethod("GetApplicationInstance", BindingFlags.Static | BindingFlags.NonPublic);
            recycleApplicationInstanceMethod = httpApplicationFactory.GetMethod("RecycleApplicationInstance", BindingFlags.Static | BindingFlags.NonPublic);
        }

        private static HttpApplication InitializeApplication()
        {
            var instance = GetApplicationInstance();
            instance.PostRequestHandlerExecute += delegate
            {
                // Collect references to context objects that would otherwise be lost when the request is completed.
                if (CrowbarContext.HttpSessionState == null)
                {
                    CrowbarContext.HttpSessionState = HttpContext.Current.Session;
                }

                if (CrowbarContext.HttpResponse == null)
                {
                    CrowbarContext.HttpResponse = HttpContext.Current.Response;
                }
            };

            RefreshEventsList(instance);
            RecycleApplicationInstance(instance);

            return instance;
        }

        private static HttpApplication GetApplicationInstance()
        {
            var request = new SimpleWorkerRequest("", "", new StringWriter());
            var context = new HttpContext(request);

            return (HttpApplication)getApplicationInstanceMethod.Invoke(null, new object[] { context });
        }

        private static void RefreshEventsList(HttpApplication appInstance)
        {
            object stepManager = typeof(HttpApplication).GetField("_stepManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(appInstance);
            object resumeStepsWaitCallback = typeof(HttpApplication).GetField("_resumeStepsWaitCallback", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(appInstance);
            var buildStepsMethod = stepManager.GetType().GetMethod("BuildSteps", BindingFlags.NonPublic | BindingFlags.Instance);
            buildStepsMethod.Invoke(stepManager, new[] { resumeStepsWaitCallback });
        }

        private static void RecycleApplicationInstance(HttpApplication appInstance)
        {
            recycleApplicationInstanceMethod.Invoke(null, new object[] { appInstance });
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <param name="project">The project path provider.</param>
        /// <param name="config">The configuration file provider.</param>
        /// <param name="defaults">The default browser context settings, if any.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication Create(IPathProvider project, IPathProvider config, Action<BrowserContext> defaults = null)
        {
            var proxy = Create<MvcApplicationProxy>(project, config, defaults);
            return new MvcApplication(proxy);
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <typeparam name="TProxy">The proxy type of the MVC application.</typeparam>
        /// <typeparam name="TContext">The proxy context type.</typeparam>
        /// <param name="project">The project path provider.</param>
        /// <param name="config">The configuration file provider.</param>
        /// <param name="defaults">The default browser context settings, if any.</param>
        /// <returns>The MVC application.</returns>
        public static MvcApplication<TContext> Create<TProxy, TContext>(IPathProvider project, IPathProvider config, Action<BrowserContext> defaults = null)
            where TProxy : MvcApplicationProxyBase<TContext>
            where TContext : IDisposable
        {
            var proxy = Create<TProxy>(project, config, defaults);
            return new MvcApplication<TContext>(proxy);
        }

        private static TProxy Create<TProxy>(IPathProvider project, IPathProvider config, Action<BrowserContext> defaults)
            where TProxy : ProxyBase
        {
            Ensure.NotNull(project, "project");
            Ensure.NotNull(config, "config");

            string configPath = config.GetPhysicalPath();

            var proxy = MvcApplicationProxyFactory.Create<TProxy>(project);
            
            proxy.Initialize(
                new SerializableDelegate<Func<HttpApplication>>(() =>
                {
                    SetCustomConfigurationFile(configPath);
                    FilterProviders.Providers.Add(new InterceptionFilterProvider());

                    return InitializeApplication();
                }),
                AppDomain.CurrentDomain.BaseDirectory,
                defaults != null ? new SerializableDelegate<Action<BrowserContext>>(defaults) : null
            );

            return proxy;
        }

        private static void SetCustomConfigurationFile(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath))
            {
                // Use default Web.config.
                return;
            }

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configPath);
            typeof(ConfigurationManager).GetField("s_initState", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 0 /* InitState.NotStarted */);
        }
    }
}