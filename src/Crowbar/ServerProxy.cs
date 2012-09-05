using System;
using Raven.Client;
using Raven.Client.Embedded;

namespace Crowbar
{
    internal class ServerProxy : MarshalByRefObject
    {
        private readonly IDocumentStore store;

        public ServerProxy()
        {
            store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }

        public void Initialize(string configurationFile, Action<string, IDocumentStore> action)
        {
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