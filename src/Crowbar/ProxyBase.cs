using System;
using System.Linq;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any MVC application proxy.
    /// </summary>
    public abstract class ProxyBase : MarshalByRefObject
    {
        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="directory">The directory in which the test is run.</param>
        /// <param name="defaults">The default browser context settings.</param>
        public abstract void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory, SerializableDelegate<Action<BrowserContext>> defaults = null);

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
        /// Creates a new browser object.
        /// </summary>
        /// <param name="defaults"></param>
        /// <returns>A browser.</returns>
        protected virtual Browser CreateBrowser(Action<BrowserContext> defaults = null)
        {
            int version = GetMvcMajorVersion();
            return new Browser(version, defaults);
        }

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