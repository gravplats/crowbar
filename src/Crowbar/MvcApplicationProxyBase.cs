using System;
using System.Web;

namespace Crowbar
{
    public abstract class MvcApplicationProxyBase<TContext> : MarshalByRefObject, IMvcApplicationProxy
        where TContext : IDisposable
    {
        private HttpApplication application;

        public void Initialize(SerializableDelegate<Func<HttpApplication>> initialize)
        {
            application = initialize.Delegate();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Process(SerializableDelegate<Action<Browser, TContext>> script)
        {
            using (var context = CreateContext(application))
            {
                script.Delegate(new Browser(), context);
            }
        }

        protected abstract TContext CreateContext(HttpApplication application);
    }
}