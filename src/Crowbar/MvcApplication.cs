using System;

namespace Crowbar
{
    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    public class MvcApplication
    {
        private readonly MvcApplicationProxy proxy;

        internal MvcApplication(MvcApplicationProxy proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Executes the specified test.
        /// </summary>
        /// <param name="test">The test to be executed.</param>
        public void Execute(Action<Browser> test)
        {
            proxy.Process(new SerializableDelegate<Action<Browser>>(test));

        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="defaults">The default browser context settings.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication Create(string name, string config = "Web.config", Action<BrowserContext> defaults = null)
        {
            return MvcApplicationFactory.Create(name, config, defaults);
        }

        /// <summary>
        /// Creates an MVC application.
        /// </summary>
        /// <typeparam name="TProxy">The proxy type of the MVC application.</typeparam>
        /// <typeparam name="TContext">The proxy context type.</typeparam>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="defaults">The default browser context settings.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication<TContext> Create<TProxy, TContext>(string name, string config = "Web.config", Action<BrowserContext> defaults = null)
            where TProxy : MvcApplicationProxyBase<TContext>
            where TContext : IDisposable
        {
            return MvcApplicationFactory.Create<TProxy, TContext>(name, config, defaults);
        }
        
        public static MvcApplication Create(string mvcProjectPath, string mvcSolutionName, string configPath, string config)
        {
            if (config == null) throw new ArgumentNullException("config");
            return MvcApplicationFactory.Create(mvcProjectPath, mvcSolutionName, configPath, config);
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

        internal MvcApplication(MvcApplicationProxyBase<TContext> proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Executes the specified test.
        /// </summary>
        /// <param name="test">The test to be executed.</param>
        public void Execute(Action<Browser, TContext> test)
        {
            proxy.Process(new SerializableDelegate<Action<Browser, TContext>>(test));
        }
    }
}