using System;
using Raven.Client;

namespace Raven.Tests
{
    public class RavenProxyContext : IDisposable
    {
        public RavenProxyContext(IDocumentStore store)
        {
            Store = store;
        }

        public IDocumentStore Store { get; private set; }

        public void Dispose()
        {
            Store.Dispose();
        }
    }
}