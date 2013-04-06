using System;
using System.Web;

namespace Crowbar
{
    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    public class MvcApplication
    {
        private readonly MvcApplication<HttpApplication> application;

        /// <summary>
        /// Creates a new instance of <see cref="MvcApplication"/>.
        /// </summary>
        /// <param name="application">The application.</param>
        public MvcApplication(MvcApplication<HttpApplication> application)
        {
            this.application = Ensure.NotNull(application, "proxy");
        }

        /// <summary>
        /// Executes the specified test.
        /// </summary>
        /// <param name="test">The test to be executed.</param>
        public void Execute(Action<Browser> test)
        {
            application.Execute(test);
        }

        /// <summary>
        /// Facade for creating an MVC application.
        /// </summary>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="defaults">The default browser context settings, if any.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication Create(string name, string config = "Web.config", Action<BrowserContext> defaults = null)
        {
            var application = Create<HttpApplication>(name, config, defaults);
            return new MvcApplication(application);
        }

        /// <summary>
        /// Facade for creating an MVC application.
        /// </summary>
        /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="defaults">The default browser context settings, if any.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication<THttpApplication> Create<THttpApplication>(string name, string config = "Web.config", Action<BrowserContext> defaults = null)
            where THttpApplication : HttpApplication
        {
            return MvcApplicationFactory.Create<THttpApplication>(new WebProjectPathProvider(name), new WebConfigPathProvider(config), defaults);
        }

        /// <summary>
        /// Facade for creating an MVC application with a proxy context.
        /// </summary>
        /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
        /// <typeparam name="TProxy">The proxy type of the MVC application.</typeparam>
        /// <typeparam name="TContext">The proxy context type.</typeparam>
        /// <param name="name">The name of the ASP.NET project.</param>
        /// <param name="config"> The name of a custom configuration file (must be set as 'Copy to Output Directory'), if null then the default 'Web.config' for the MVC project will be used. </param>
        /// <param name="defaults">The default browser context settings, if any.</param>
        /// <returns>An MVC application.</returns>
        public static MvcApplication<THttpApplication, TContext> Create<THttpApplication, TProxy, TContext>(string name, string config = "Web.config", Action<BrowserContext> defaults = null)
            where THttpApplication : HttpApplication
            where TProxy : MvcApplicationProxyBase<THttpApplication, TContext>
            where TContext : IDisposable
        {
            return MvcApplicationFactory.Create<THttpApplication, TProxy, TContext>(new WebProjectPathProvider(name), new WebConfigPathProvider(config), defaults);
        }
    }

    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    public class MvcApplication<THttpApplication>
        where THttpApplication : HttpApplication
    {
        private readonly MvcApplicationProxy<THttpApplication> proxy;

        /// <summary>
        /// Creates a new instance of <see cref="MvcApplication"/>.
        /// </summary>
        /// <param name="proxy">The application proxy.</param>
        public MvcApplication(MvcApplicationProxy<THttpApplication> proxy)
        {
            this.proxy = Ensure.NotNull(proxy, "proxy");
        }

        /// <summary>
        /// Executes the specified test.
        /// </summary>
        /// <param name="test">The test to be executed.</param>
        public void Execute(Action<Browser> test)
        {
            proxy.Process(new SerializableDelegate<Action<Browser>>(test));
        }
    }


    /// <summary>
    /// Represents a proxy to an MVC application.
    /// </summary>
    /// <typeparam name="THttpApplication">The HTTP application type.</typeparam>
    /// <typeparam name="TContext">The proxy context type.</typeparam>
    public class MvcApplication<THttpApplication, TContext>
        where THttpApplication : HttpApplication
        where TContext : IDisposable
    {
        private readonly MvcApplicationProxyBase<THttpApplication, TContext> proxy;

        /// <summary>
        /// Creates a new instance of <see cref="MvcApplication{THttpApplication, TContext}"/>.
        /// </summary>
        /// <param name="proxy">The application proxy.</param>
        public MvcApplication(MvcApplicationProxyBase<THttpApplication, TContext> proxy)
        {
            this.proxy = Ensure.NotNull(proxy, "proxy");
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