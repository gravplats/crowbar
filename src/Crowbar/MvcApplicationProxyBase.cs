using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common base class for any generic MVC application proxy.
    /// </summary>
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    /// <typeparam name="TContext">The type of the user-defined context.</typeparam>
    public abstract class MvcApplicationProxyBase<THttpApplication, TContext> : ProxyBase<THttpApplication, Action<Client, TContext>>
        where THttpApplication : HttpApplication
        where TContext : IDisposable
    {
        /// <inheritdoc />
        protected override void ProcessCore(SerializableDelegate<Action<Client, TContext>> script, THttpApplication application, string testBaseDirectory, IHttpPayloadDefaults defaults)
        {
            try
            {
                using (var context = CreateContext(application, testBaseDirectory))
                {
                    var client = CreateClient(application, testBaseDirectory, defaults, context);
                    script.Delegate(client, context);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        /// <summary>
        /// Creates a new client object.
        /// </summary>
        /// <param name="application">The HTTP application.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        /// <param name="defaults">Default HTTP payload settings, if any.</param>
        /// <param name="context">The proxy context.</param>
        /// <returns>A client.</returns>
        protected virtual Client CreateClient(THttpApplication application, string testBaseDirectory, IHttpPayloadDefaults defaults, TContext context)
        {
            return new Client(defaults);
        }

        /// <summary>
        /// Creates the proxy context.
        /// </summary>
        /// <param name="application">The HTTP application.</param>
        /// <param name="testBaseDirectory">The directory in which the test is run.</param>
        /// <returns>The proxy context.</returns>
        protected abstract TContext CreateContext(THttpApplication application, string testBaseDirectory);
    }
}