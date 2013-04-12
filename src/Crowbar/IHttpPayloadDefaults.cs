namespace Crowbar
{
    /// <summary>
    /// Provides functionality for specifying the default HTTP payload settings.
    /// </summary>
    public interface IHttpPayloadDefaults
    {
        /// <summary>
        /// Applies any default HTTP payload settings to the HTTP payload object.
        /// </summary>
        /// <param name="payload">The HTTP payload.</param>
        void ApplyTo(HttpPayload payload);
    }
}