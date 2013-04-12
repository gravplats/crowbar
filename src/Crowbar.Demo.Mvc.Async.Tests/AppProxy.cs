using Crowbar.Demo.Mvc.Async.Application;

namespace Crowbar.Demo.Mvc.Async.Tests
{
    public class AppProxy : MvcApplicationProxyBase<App, AppProxyContext>
    {
        protected override void OnApplicationStart(App application, string testBaseDirectory)
        {
            application.Kernel.Rebind<IExternalRequestAsync>().ToConstant(new AppProxyExternalRequestAsync());
        }

        protected override AppProxyContext CreateContext(App application, string testBaseDirectory)
        {
            return new AppProxyContext();
        }
    }
}