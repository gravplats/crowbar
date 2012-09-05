using Raven.Client;

namespace Crowbar
{
    public interface IRavenDbHttpApplication
    {
        IDocumentStore Store { set; }
    }
}