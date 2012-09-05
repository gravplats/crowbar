using System;
using Crowbar.Browsing;
using Raven.Client;
using Raven.Client.Embedded;

namespace Crowbar.Hosting
{
    internal class BrowserProxy : MarshalByRefObject
    {
        private readonly IDocumentStore store;

        public BrowserProxy()
        {
            store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }

        public void RunCodeInAppDomain(Action<IDocumentStore> action)
        {
            action(store);
        }

        public void RunBrowsingSessionInAppDomain(SerializableDelegate<Action<BrowserSession>> script)
        {
            var browsingSession = new BrowserSession(store);
            script.Delegate(browsingSession);
        }

        public override object InitializeLifetimeService()
        {
            // Tells .NET not to expire this remoting object.
            return null; 
        }
    }
}