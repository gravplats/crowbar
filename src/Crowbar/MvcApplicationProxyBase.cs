using System;
using System.Web;

namespace Crowbar
{
    public abstract class MvcApplicationProxyBase<TContext> : MarshalByRefObject
    {
        private HttpApplication application;

        public void Initialize(string config, Func<string, HttpApplication> initialize)
        {
            application = initialize(config);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Process(SerializableDelegate<Action<TContext, Browser>> script)
        {
            var context = CreateContext(application);
            script.Delegate(context, new Browser());

            var disposable = context as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        protected abstract TContext CreateContext(HttpApplication application);
    }
}