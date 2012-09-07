using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Crowbar
{
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

        private static void SetCustomConfigurationFile(string configFile)
        {
            if (configFile == null)
            {
                // Use the default Web.config.
                return;
            }

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFile);
            typeof(ConfigurationManager).GetField("s_initState", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 0 /* InitState.NotStarted */);
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

                if (CrowbarContext.Response == null)
                {
                    CrowbarContext.Response = HttpContext.Current.Response;
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
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <returns></returns>
        public static MvcApplication<object> Create(string name, string config = "Web.config")
        {
            return Create<MvcApplicationProxy, object>(name, config);
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <typeparam name="TProxy">The proxy type of the MVC application.</typeparam>
        /// <typeparam name="TContext">The proxy context type.</typeparam>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <returns></returns>
        public static MvcApplication<TContext> Create<TProxy, TContext>(string name, string config = "Web.config")
            where TProxy : MvcApplicationProxyBase<TContext>
        {
            config = config == null ? null : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config);

            var proxy = MvcApplicationProxyFactory.Create<TProxy, TContext>(name);
            proxy.Initialize(new SerializableDelegate<Func<HttpApplication>>(() =>
            {
                SetCustomConfigurationFile(config);
                FilterProviders.Providers.Add(new InterceptionFilterProvider());

                return InitializeApplication();
            }));

            return new MvcApplication<TContext>(proxy);
        }
    }
}