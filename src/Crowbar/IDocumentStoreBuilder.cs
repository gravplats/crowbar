using Raven.Client;

namespace Crowbar
{
    /// <summary>
    /// Defines functionality for building a document store.
    /// </summary>
    public interface IDocumentStoreBuilder
    {
        /// <summary>
        /// Builds an initialized document store.
        /// </summary>
        /// <returns>A document store.</returns>
        IDocumentStore Build();
    }
}