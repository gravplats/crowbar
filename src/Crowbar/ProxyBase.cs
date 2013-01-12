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
        public abstract void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory);

        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected void HandleException(Exception exception)
        {
            if (exception.IsSerializable())
            {
                throw new CrowbarException("An exception was thrown during the execution of the test.", exception);
            }

            throw new Exception(string.Format("An exception was thrown during the execution of the test: {0}", exception.Message));
        }

        protected Browser CreateBrowser()
        {
            int version = GetMvcMajorVersion();
            return new Browser(version);
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