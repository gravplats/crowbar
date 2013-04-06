using Crowbar;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;
using Raven.Web;

namespace Raven.Tests
{
    public class RavenProxy : MvcApplicationProxyBase<RavenMvcApplication, RavenProxyContext>
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

        protected override RavenProxyContext CreateContext(RavenMvcApplication application, string testBaseDirectory)
        {
            var store = CreateDocumentStore();
            application.SetDocumentStore(store);

            return new RavenProxyContext(store);
        }
    }
}