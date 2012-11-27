using System.Web;
using Crowbar.Mvc.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace Crowbar.Tests
{
    public class RavenMvcApplicationProxy : MvcApplicationProxyBase<RavenContext>
    {
        public class WaitForNonStaleResultsListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
            {
                customization.WaitForNonStaleResults();
            }
        }

        protected override RavenContext CreateContext(HttpApplication application, string testBaseDirectory)
        {
            var store = CreateDocumentStore();

            var crowbar = (ICrowbarHttpApplication)application;
            crowbar.SetDocumentStore(store);

            return new RavenContext(store);
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
    }
}