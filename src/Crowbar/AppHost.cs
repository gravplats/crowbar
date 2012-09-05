using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Raven.Client;

namespace Crowbar
{
    /// <summary>
    /// Hosts an ASP.NET application within an ASP.NET-enabled .NET appdomain
    /// and provides methods for executing test code within that appdomain
    /// </summary>
    public class AppHost
    {
        private readonly BrowserProxy browserProxy; // The gateway to the ASP.NET-enabled .NET appdomain

        private AppHost(string appPhysicalDirectory, string virtualDirectory = "/")
        {
            browserProxy = (BrowserProxy)ApplicationHost.CreateApplicationHost(typeof(BrowserProxy), virtualDirectory, appPhysicalDirectory);
            browserProxy.RunCodeInAppDomain(store =>
            {
                InitializeApplication(store);
                FilterProviders.Providers.Add(new InterceptionFilterProvider());
                LastRequestData.Reset();
            });
        }

        public void Start(Action<BrowserSession> testScript)
        {
            var serializableDelegate = new SerializableDelegate<Action<BrowserSession>>(testScript);
            browserProxy.RunBrowsingSessionInAppDomain(serializableDelegate);
        }

        #region Initializing app & interceptors
        private static void InitializeApplication(IDocumentStore store)
        {
            var appInstance = GetApplicationInstance();

            var ravenDbHttpApplication = (IRavenDbHttpApplication)appInstance;
            ravenDbHttpApplication.Store = store;

            appInstance.PostRequestHandlerExecute += delegate
            {
                // Collect references to context objects that would otherwise be lost
                // when the request is completed
                if (LastRequestData.HttpSessionState == null)
                    LastRequestData.HttpSessionState = HttpContext.Current.Session;
                if (LastRequestData.Response == null)
                    LastRequestData.Response = HttpContext.Current.Response;
            };
            RefreshEventsList(appInstance);

            RecycleApplicationInstance(appInstance);
        }
        #endregion

        #region Reflection hacks
        private static readonly MethodInfo getApplicationInstanceMethod;
        private static readonly MethodInfo recycleApplicationInstanceMethod;

        static AppHost()
        {
            // Get references to some MethodInfos we'll need to use later to bypass nonpublic access restrictions
            var httpApplicationFactory = typeof(HttpContext).Assembly.GetType("System.Web.HttpApplicationFactory", true);
            getApplicationInstanceMethod = httpApplicationFactory.GetMethod("GetApplicationInstance", BindingFlags.Static | BindingFlags.NonPublic);
            recycleApplicationInstanceMethod = httpApplicationFactory.GetMethod("RecycleApplicationInstance", BindingFlags.Static | BindingFlags.NonPublic);
        }

        private static HttpApplication GetApplicationInstance()
        {
            var writer = new StringWriter();
            var workerRequest = new SimpleWorkerRequest("", "", writer);
            var httpContext = new HttpContext(workerRequest);
            return (HttpApplication)getApplicationInstanceMethod.Invoke(null, new object[] { httpContext });
        }

        private static void RecycleApplicationInstance(HttpApplication appInstance)
        {
            recycleApplicationInstanceMethod.Invoke(null, new object[] { appInstance });
        }

        private static void RefreshEventsList(HttpApplication appInstance)
        {
            object stepManager = typeof(HttpApplication).GetField("_stepManager", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(appInstance);
            object resumeStepsWaitCallback = typeof(HttpApplication).GetField("_resumeStepsWaitCallback", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(appInstance);
            var buildStepsMethod = stepManager.GetType().GetMethod("BuildSteps", BindingFlags.NonPublic | BindingFlags.Instance);
            buildStepsMethod.Invoke(stepManager, new[] { resumeStepsWaitCallback });
        }

        #endregion

        /// <summary>
        /// Creates an instance of the AppHost so it can be used to simulate a browsing session.
        /// </summary>
        /// <returns></returns>
        public static AppHost Simulate(string mvcProjectDirectory)
        {
            var mvcProjectPath = GetMvcProjectPath(mvcProjectDirectory);
            if (mvcProjectPath == null)
            {
                throw new ArgumentException(string.Format("Mvc Project {0} not found", mvcProjectDirectory));
            }
            CopyDllFiles(mvcProjectPath);
            return new AppHost(mvcProjectPath);
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

        private static string GetMvcProjectPath(string mvcProjectName)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDirectory.Contains("\\"))
            {
                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\"));
                var mvcPath = Path.Combine(baseDirectory, mvcProjectName);
                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }
            }
            return null;
        }
    }
}