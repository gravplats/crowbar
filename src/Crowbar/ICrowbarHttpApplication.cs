using Raven.Client;

namespace Crowbar
{
    /// <summary>
    /// Defines functionality for setting the document store of an MVC application.
    /// </summary>
    public interface ICrowbarHttpApplication
    {
        /// <summary>
        /// Sets the document store.
        /// </summary>
        /// <param name="store">The document store.</param>
        void SetDocumentStore(IDocumentStore store);
    }
}