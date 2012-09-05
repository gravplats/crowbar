using System;
using Raven.Client;

namespace Crowbar
{
    internal class MvcApplicationProxy : MarshalByRefObject
    {
        private IDocumentStore store;

        public void Initialize(string configurationFile, IDocumentStoreBuilder builder, Action<string, IDocumentStore> action)
        {
            store = builder.Build();
            action(configurationFile, store);
        }

        public override object InitializeLifetimeService()
        {
            // Tells .NET not to expire this remoting object.
            return null; 
        }

        public void Process(SerializableDelegate<Action<ServerContext, Browser>> script)
        {
            script.Delegate(new ServerContext(store), new Browser());
        }
    }
}