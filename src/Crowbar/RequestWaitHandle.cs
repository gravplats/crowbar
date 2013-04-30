using System.Threading;

namespace Crowbar
{
    /// <summary>
    /// The default HTTP request synchronization implementation.
    /// </summary>
    public class RequestWaitHandle : IRequestWaitHandle
    {
        private readonly ManualResetEventSlim evt;

        /// <summary>
        /// Creates an instance of <see cref="RequestWaitHandle"/>.
        /// </summary>
        public RequestWaitHandle()
        {
            evt = new ManualResetEventSlim(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            evt.Dispose();
        }

        /// <inheritdoc />
        public void Signal()
        {
            evt.Set();
        }

        /// <inheritdoc />
        public void Wait()
        {
            evt.Wait();
        }
    }
}