using System;
using Raven.Client;
using Raven.Client.Embedded;

namespace Crowbar
{
    public class ServerProxy : MarshalByRefObject
    {
        private readonly IDocumentStore store;

        public ServerProxy()
        {
            store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }

        public void Initialize(Action<IDocumentStore> action)
        {
            action(store);
        }

        public override object InitializeLifetimeService()
        {
            // Tells .NET not to expire this remoting object.
            return null; 
        }

        public void Process(SerializableDelegate<Action<ServerContext>> script)
        {
            var context = new ServerContext(store);
            script.Delegate(context);
        }
    }
}