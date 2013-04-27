using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// A common implementation for a non-generic MVC application proxy.
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    /// </summary>
    public class MvcApplicationProxy<THttpApplication> : ProxyBase<THttpApplication, Action<Client>>
        where THttpApplication : HttpApplication
    {
        /// <inheritdoc />
        protected override void ProcessCore(SerializableDelegate<Action<Client>> script, THttpApplication application, string testBaseDirectory, IHttpPayloadDefaults defaults)
        {
            try
            {
                var client = CreateClient(application, testBaseDirectory, defaults);
                script.Delegate(client);
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
        /// <returns>A client.</returns>
        protected virtual Client CreateClient(THttpApplication application, string testBaseDirectory, IHttpPayloadDefaults defaults)
        {
            return new Client(testBaseDirectory, defaults);
        }
    }
}