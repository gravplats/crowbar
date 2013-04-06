using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common implementation for a non-generic MVC application proxy.
    /// </summary>
    public class MvcApplicationProxy : ProxyBase<Action<Browser>>
    {
        /// <inheritdoc />
        protected override void ProcessCore(SerializableDelegate<Action<Browser>> script, HttpApplication application, Browser browser, string testBaseDirectory)
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