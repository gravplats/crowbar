using Raven.Client;

namespace Crowbar
{
    /// <summary>
    /// Defines the server context.
    /// </summary>
    public class ServerContext
    {
        public ServerContext(IDocumentStore store)
        {
            Store = store;
        }

        /// <summary>
        /// Gets the RavenDB document store.
        /// </summary>
        public IDocumentStore Store { get; private set; }
    }
}