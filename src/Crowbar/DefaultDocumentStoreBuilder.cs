using System;
using Crowbar.Mvc;
using Raven.Client;
using Raven.Client.Embedded;

namespace Crowbar
{
    [Serializable]
    public class DefaultDocumentStoreBuilder : IDocumentStoreBuilder
    {
        public IDocumentStore Build()
        {
            return new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }
    }
}