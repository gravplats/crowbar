using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common implementation for a non-generic MVC application proxy.
    /// </summary>
    public class MvcApplicationProxy : ProxyBase
    {
        /// <summary>
        /// Initializes the proxy.
        /// </summary>
        /// <param name="initialize">The initialization code.</param>
        /// <param name="directory">The directory in which the test is run.</param>
        public override void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory)
        {
            initialize.Delegate();
        }

        /// <summary>
        /// Runs the test script against the MVC application proxy.
        /// </summary>
        /// <param name="script">The test script to be run.</param>
        public void Process(SerializableDelegate<Action<Browser>> script)
        {
            try
            {
                var browser = CreateBrowser();
                script.Delegate(browser);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}