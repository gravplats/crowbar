using System;
using Crowbar.Mvc;

namespace Crowbar
{
    internal class MvcApplicationProxy : MarshalByRefObject
    {
        private IDocumentStoreBuilder builder;
        private ICrowbarHttpApplication application;

        public void Initialize(string config, IDocumentStoreBuilder documentStoreBuilder, Func<string, ICrowbarHttpApplication> initialize)
        {
            builder = documentStoreBuilder;
            application = initialize(config);
        }

        public override object InitializeLifetimeService()
        {
            // Tells .NET not to expire this remoting object.
            return null;
        }

        public void Process(SerializableDelegate<Action<ServerContext, Browser>> script)
        {
            using (var store = builder.Build())
            {
                application.SetDocumentStore(store);
                script.Delegate(new ServerContext(store), new Browser());
            }
        }
    }
}