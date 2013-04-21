using System;
using Raven.Client;

namespace Crowbar.Demo.Mvc.Raven.Tests
{
    public class AppProxyContext : IDisposable
    {
        public AppProxyContext(IDocumentStore store)
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