using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common implementation for a non-generic MVC application proxy.
    /// </summary>
    public class MvcApplicationProxy : ProxyBase
    {
        private Action<BrowserContext> defaults;

        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="directory">The directory in which the test is run.</param>
        /// <param name="defaults">The default browser context settings.</param>
        public override void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory, SerializableDelegate<Action<BrowserContext>> defaults = null)
        {
            initialize.Delegate();
            this.defaults = (defaults != null) ? defaults.Delegate : null;
        }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        public void Process(SerializableDelegate<Action<Browser>> script)
        {
            try
            {
                var browser = CreateBrowser(defaults);
                script.Delegate(browser);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}