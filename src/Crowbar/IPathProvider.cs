namespace Crowbar
{
    /// <summary>
    /// Provides functionality for returning the physical path of a resource.
    /// </summary>
    public interface IPathProvider
    {
        /// <summary>
        /// Returns the physical path of the underlying resource.
        /// </summary>
        /// <returns>The physical path.</returns>
        string GetPhysicalPath();
    }
}