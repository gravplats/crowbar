using System;
using Raven.Client;

namespace Crowbar.Tests
{
    public class RavenContext : IDisposable
    {
        public RavenContext(IDocumentStore store)
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