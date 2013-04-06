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
        protected override void ProcessCore(SerializableDelegate<Action<Client>> script, THttpApplication application, Client client, string testBaseDirectory)
        {
            try
            {
                script.Delegate(client);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}