using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any generic MVC application proxy.
    /// </summary>
    /// <typeparam name="TContext">The type of the user-defined context.</typeparam>
    public abstract class MvcApplicationProxyBase<TContext> : ProxyBase
        where TContext : IDisposable
    {
        private HttpApplication application;
        private string testBaseDirectory;

        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="directory">The directory in which the test is run.</param>
        public override void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory)
        {
            application = initialize.Delegate();
            testBaseDirectory = directory;
        }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        public void Process(SerializableDelegate<Action<Browser, TContext>> script)
        {
            try
            {
                using (var context = CreateContext(application, testBaseDirectory))
                {
                    var browser = CreateBrowser();
                    script.Delegate(browser, context);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        /// <summary>
        /// Creates the proxy context.
        /// </summary>
        /// <param name="application">The HTTP application.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        /// <returns>The proxy context.</returns>
        protected abstract TContext CreateContext(HttpApplication application, string testBaseDirectory);
    }
}