using Raven.Client;

namespace Crowbar
{
    public class ServerContext
    {
        public ServerContext(IDocumentStore store)
        {
            Store = store;
        }

        public IDocumentStore Store { get; private set; }
    }
}