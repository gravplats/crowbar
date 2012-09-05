using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Crowbar.Mvc;
using Raven.Client;

namespace Crowbar
{
    internal class MvcApplicationFactory
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

        private static void InitializeApplication(IDocumentStore store)
        {
            var instance = GetApplicationInstance();
            ((ICrowbarHttpApplication)instance).SetDocumentStore(store);

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

        public static MvcApplication Create(string name, string configurationFile, IDocumentStoreBuilder documentStoreBuilder)
        {
            var physicalPath = GetPhysicalPath(name);
            if (physicalPath == null)
            {
                throw new ArgumentException(string.Format("Mvc Project {0} not found", name));
            }

            CopyDllFiles(physicalPath);

            var proxy = (MvcApplicationProxy)ApplicationHost.CreateApplicationHost(typeof(MvcApplicationProxy), "/", physicalPath);
            configurationFile = configurationFile == null ? null : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFile);

            proxy.Initialize(configurationFile, documentStoreBuilder, (configFile, store) =>
            {
                SetCustomConfigurationFile(configFile);
                InitializeApplication(store);
                FilterProviders.Providers.Add(new InterceptionFilterProvider());
                CrowbarContext.Reset();
            });

            return new MvcApplication(proxy);
        }

        private static string GetPhysicalPath(string mvcProjectName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDirectory.Contains("\\"))
            {
                string mvcPath = Path.Combine(baseDirectory, mvcProjectName);
                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }

                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\"));
            }

            return null;
        }

        private static void CopyDllFiles(string mvcProjectPath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var file in Directory.GetFiles(baseDirectory, "*.dll"))
            {
                var destFile = Path.Combine(mvcProjectPath, "bin", Path.GetFileName(file));
                if (!File.Exists(destFile) || File.GetCreationTimeUtc(destFile) != File.GetCreationTimeUtc(file))
                {
                    File.Copy(file, destFile, true);
                }
            }
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
    }
}