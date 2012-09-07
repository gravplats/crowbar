using System;

namespace Crowbar
{
    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    public class MvcApplication
    {
        private readonly MvcApplicationProxy proxy;

        public MvcApplication(MvcApplicationProxy proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Executes the specified test script.
        /// </summary>
        /// <param name="script">The test script to be executed.</param>
        public void Execute(Action<Browser> script)
        {
            proxy.Process(new SerializableDelegate<Action<Browser>>(script));
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication Create(string name, string config = "Web.config")
        {
            return MvcApplicationFactory.Create(name, config);
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <typeparam name="TProxy">The proxy type of the MVC application.</typeparam>
        /// <typeparam name="TContext">The proxy context type.</typeparam>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication<TContext> Create<TProxy, TContext>(string name, string config = "Web.config")
            where TProxy : MvcApplicationProxyBase<TContext>
            where TContext : IDisposable
        {
            return MvcApplicationFactory.Create<TProxy, TContext>(name, config);
        }
    }


    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    /// <typeparam name="TContext">The proxy context type.</typeparam>
    public class MvcApplication<TContext>
        where TContext : IDisposable
    {
        private readonly MvcApplicationProxyBase<TContext> proxy;

        public MvcApplication(MvcApplicationProxyBase<TContext> proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Executes the specified test script.
        /// </summary>
        /// <param name="script">The test script to be executed.</param>
        public void Execute(Action<TContext, Browser> script)
        {
            proxy.Process(new SerializableDelegate<Action<TContext, Browser>>(script));
        }
    }
}