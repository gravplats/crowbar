using Ninject.Modules;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Listeners;

namespace Crowbar.Demo.Mvc.Raven.Tests.Infrastructure.Modules
{
    public class EmbeddableRavenModule : NinjectModule
    {
        public class WaitForNonStaleResultsListener : IDocumentQueryListener
        {
            public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
            {
                customization.WaitForNonStaleResults();
            }
        }

        public override void Load()
        {
            var store = CreateDocumentStore().Initialize();
            Bind<IDocumentStore>().ToConstant(store).InSingletonScope();
        }

        private IDocumentStore CreateDocumentStore()
        {
            var store = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInMemory = true,
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true
                }
            };

            return store.RegisterListener(new WaitForNonStaleResultsListener());
        }
    }
}