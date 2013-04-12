using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any MVC application proxy.
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    /// <typeparam name="TDelegate">The script delegate type.</typeparam>
    /// </summary>
    public abstract class ProxyBase<THttpApplication, TDelegate> : MarshalByRefObject, IProxy
        where THttpApplication : HttpApplication
        where TDelegate : class
    {
        private THttpApplication httpApplication;
        private string directory;
        private IHttpPayloadDefaults httpPayloadDefaults;

        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        /// <param name="defaults">Default HTTP payload settings, if any.</param>
        public void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string testBaseDirectory, IHttpPayloadDefaults defaults = null)
        {
            httpApplication = initialize.Delegate() as THttpApplication;
            if (httpApplication == null)
            {
                string message = string.Format("The HTTP application is not of type '{0}'.", typeof(THttpApplication));
                throw new InvalidOperationException(message);
            }

            directory = testBaseDirectory;
            httpPayloadDefaults = defaults;

            OnApplicationStart(httpApplication, directory);
        }

        /// <inheritdoc />
        public override object InitializeLifetimeService()
        {
            return null;
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
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        protected virtual void OnApplicationStart(THttpApplication application, string testBaseDirectory) { }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        public void Process(SerializableDelegate<TDelegate> script)
        {
            Ensure.NotNull(script, "script");
            ProcessCore(script, httpApplication, directory, httpPayloadDefaults);
        }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        /// <param name="application">The HTTP application.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        /// <param name="defaults">Default HTTP payload settings, if any.</param>
        protected abstract void ProcessCore(SerializableDelegate<TDelegate> script, THttpApplication application, string testBaseDirectory, IHttpPayloadDefaults defaults);
    }
}