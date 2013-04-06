using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any generic MVC application proxy.
    /// </summary>
    /// <typeparam name="TContext">The type of the user-defined context.</typeparam>
    public abstract class MvcApplicationProxyBase<TContext> : ProxyBase<Action<Browser, TContext>>
        where TContext : IDisposable
    {
        /// <inheritdoc />
        protected override void ProcessCore(SerializableDelegate<Action<Browser, TContext>> script, HttpApplication application, Browser browser, string testBaseDirectory)
        {
            try
            {
                using (var context = CreateContext(application, testBaseDirectory))
                {
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