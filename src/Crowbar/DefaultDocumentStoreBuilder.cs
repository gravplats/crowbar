using System;
using Crowbar.Mvc;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace Crowbar
{
    [Serializable]
    public class DefaultDocumentStoreBuilder : IDocumentStoreBuilder
    {
        public class WaitForNonStaleResultsListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
            {
                customization.WaitForNonStaleResults();
            }
        }

        public IDocumentStore Build()
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