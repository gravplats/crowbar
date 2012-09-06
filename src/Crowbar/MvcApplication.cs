using System;
using Crowbar.Mvc;

namespace Crowbar
{
    /// <summary>
    /// Represents an MVC-application.
    /// </summary>
    public class MvcApplication
    {
        private readonly MvcApplicationProxy proxy;

        internal MvcApplication(MvcApplicationProxy proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Executes the specified test script.
        /// </summary>
        /// <param name="script">The test script to be executed.</param>
        public void Execute(Action<ServerContext, Browser> script)
        {
            proxy.Process(new SerializableDelegate<Action<ServerContext, Browser>>(script));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MvcApplication"/> class.
        /// </summary>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="builder">A document store builder</param>
        public static MvcApplication Create(string name, string config = "Web.config", IDocumentStoreBuilder builder = null)
        {
            if (builder != null)
            {
                var attributes = builder.GetType().GetCustomAttributes(typeof(SerializableAttribute), true);
                if (attributes.Length == 0)
                {
                    string message = string.Format("Any custom '{0}' must be marked as serializable.", typeof(IDocumentStoreBuilder));
                    throw new InvalidOperationException(message);
                }
            }

            return MvcApplicationFactory.Create(name, config, builder ?? new DefaultDocumentStoreBuilder());
        }
    }
}