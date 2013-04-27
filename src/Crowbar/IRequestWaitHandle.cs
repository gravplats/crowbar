namespace Crowbar
{
    /// <summary>
    /// Provides funtionality for synchronization of an HTTP request.
    /// </summary>
    public interface IRequestWaitHandle
    {
        /// <summary>
        /// Signals the end of an HTTP request.
        /// </summary>
        void Signal();

        /// <summary>
        /// Waits for the end of an HTTP request.
        /// </summary>
        void Wait();
    }
}