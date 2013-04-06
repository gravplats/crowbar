using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common implementation for a non-generic MVC application proxy.
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    /// </summary>
    public class MvcApplicationProxy<THttpApplication> : ProxyBase<THttpApplication, Action<Browser>>
        where THttpApplication : HttpApplication
    {
        /// <inheritdoc />
        protected override void ProcessCore(SerializableDelegate<Action<Browser>> script, THttpApplication application, Browser browser, string testBaseDirectory)
        {
            try
            {
                script.Delegate(browser);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}