using System;
using System.Linq;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any MVC application proxy.
    /// </summary>
    public abstract class ProxyBase<TDelegate> : MarshalByRefObject, IProxy
        where TDelegate : class
    {
        private HttpApplication httpApplication;
        private Browser browser;
        private string testBaseDirectory;

        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="directory">The directory in which the test is run.</param>
        /// <param name="defaults"></param>
        public void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory, SerializableDelegate<Action<BrowserContext>> defaults = null)
        {
            var action = (defaults != null) ? defaults.Delegate : null;
            browser = Ensure.NotNull(CreateBrowser(GetMvcMajorVersion(), action), "browser");

            testBaseDirectory = directory;

            httpApplication = initialize.Delegate();
            OnApplicationStart(httpApplication);
        }

        /// <inheritdoc />
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Creates a new browser object.
        /// </summary>
        /// <param name="mvcMajorVersion">The major version of the MVC framework</param>
        /// <param name="defaults">The default browser context settings.</param>
        /// <returns>A browser.</returns>
        protected virtual Browser CreateBrowser(int mvcMajorVersion, Action<BrowserContext> defaults)
        {
            return new Browser(mvcMajorVersion, defaults);
        }

        /// <summary>
        /// Handles exceptions thrown by the proxy.
        /// </summary>
        /// <param name="exception">The exception that should be handled.</param>
        protected void HandleException(Exception exception)
        {
            if (exception.IsSerializable())
            {
                throw new CrowbarException("An exception was thrown during the execution of the test.", exception);
            }

            throw new Exception(string.Format("An exception was thrown during the execution of the test: {0}", exception.Message));
        }

        /// <summary>
        /// Provides the opportunity of customizing the application post application start.
        /// </summary>
        /// <param name="application">The HTTP application.</param>
        protected virtual void OnApplicationStart(HttpApplication application) { }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        public void Process(SerializableDelegate<TDelegate> script)
        {
            ProcessCore(script, httpApplication, browser, testBaseDirectory);
        }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        /// <param name="application">The HTTP application.</param>
        /// <param name="browser">The browser object.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        protected abstract void ProcessCore(SerializableDelegate<TDelegate> script, HttpApplication application, Browser browser, string testBaseDirectory);

        private static int GetMvcMajorVersion()
        {
            var mvc = AppDomain.CurrentDomain.GetAssemblies()
                .Select(x => x.GetName())
                .FirstOrDefault(x => x.Name == "System.Web.Mvc");

            if (mvc != null)
            {
                return mvc.Version.Major;
            }

            // Uh-huh... no MVC assembly... should not work.
            return 0;
        }
    }
}