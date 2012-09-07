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

        public void Execute(Action<TContext, Browser> script)
        {
            proxy.Process(new SerializableDelegate<Action<TContext, Browser>>(script));
        }
    }
}