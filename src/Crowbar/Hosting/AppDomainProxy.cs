using System;
using Crowbar.Browsing;
using Raven.Client;
using Raven.Client.Embedded;

namespace Crowbar.Hosting
{
    /// <summary>
    /// Simply provides a remoting gateway to execute code within the ASP.NET-hosting appdomain
    /// </summary>
    internal class AppDomainProxy : MarshalByRefObject
    {
        private readonly IDocumentStore store;

        public AppDomainProxy()
        {
            store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
        }

        public void RunCodeInAppDomain(Action<IDocumentStore> codeToRun)
        {
            codeToRun(store);
        }

        public void RunBrowsingSessionInAppDomain(SerializableDelegate<Action<BrowsingSession>> script)
        {
            var browsingSession = new BrowsingSession(store);
            script.Delegate(browsingSession);
        }

        public override object InitializeLifetimeService()
        {
            return null; // Tells .NET not to expire this remoting object
        }
    }
}