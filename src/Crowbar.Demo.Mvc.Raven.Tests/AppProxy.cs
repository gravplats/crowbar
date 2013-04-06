using Crowbar.Demo.Mvc.Raven.Application;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace Crowbar.Demo.Mvc.Raven.Tests
{
    public class AppProxy : MvcApplicationProxyBase<App, AppProxyContext>
    {
        public class WaitForNonStaleResultsListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
            {
                customization.WaitForNonStaleResults();
            }
        }

        private static IDocumentStore CreateDocumentStore()
        {
            var store = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInMemory = true,
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true
                }
            };

            return store.RegisterListener(new WaitForNonStaleResultsListener()).Initialize();
        }

        protected override AppProxyContext CreateContext(App application, string testBaseDirectory)
        {
            var store = CreateDocumentStore();
            application.SetDocumentStore(store);

            return new AppProxyContext(store);
        }
    }
}