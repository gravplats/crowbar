using System;

namespace Crowbar
{
    /// <summary>
    /// Represents an MVC-application.
    /// </summary>
    public class MvcApplication<TContext>
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