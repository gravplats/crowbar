using Crowbar.Demo.Mvc.Raven.Application;
using Crowbar.Demo.Mvc.Raven.Tests.Infrastructure.Modules;
using Ninject;
using Raven.Client;

namespace Crowbar.Demo.Mvc.Raven.Tests
{
    public class AppProxy : MvcApplicationProxyBase<App, AppProxyContext>
    {
        protected override AppProxyContext CreateContext(App application, string testBaseDirectory)
        {
            // we need a refresh instance of an embeddale database for each test.
            var kernel = application.Kernel;
            kernel.Reload(new EmbeddableRavenModule());

            var store = application.Kernel.Get<IDocumentStore>();
            return new AppProxyContext(store);
        }
    }
}