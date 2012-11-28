using System;
using System.Web;

namespace Crowbar
{
    public abstract class MvcApplicationProxyBase<TContext> : ProxyBase
        where TContext : IDisposable
    {
        private HttpApplication application;
        private string testBaseDirectory;

        public override void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory)
        {
            application = initialize.Delegate();
            testBaseDirectory = directory;
        }

        public void Process(SerializableDelegate<Action<Browser, TContext>> script)
        {
            try
            {
                using (var context = CreateContext(application, testBaseDirectory))
                {
                    var browser = CreateBrowser();
                    script.Delegate(browser, context);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        protected abstract TContext CreateContext(HttpApplication application, string testBaseDirectory);
    }
}