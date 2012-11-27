using Raven.Client;

namespace Crowbar.Mvc.Common
{
    public interface ICrowbarHttpApplication 
    {
        void SetDocumentStore(IDocumentStore store);
    }
}