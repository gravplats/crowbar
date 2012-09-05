using System;

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
        /// <param name="configurationFile">
        /// The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used.
        /// </param>
        public static MvcApplication Create(string name, string configurationFile = "Web.config")
        {
            return MvcApplicationFactory.Create(name, configurationFile);
        }
    }
}