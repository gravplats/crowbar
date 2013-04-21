using System;
using System.Runtime.Serialization;

namespace Crowbar
{
    /// <summary>
    /// Exception that uses the inner exceptions stack trace.
    /// </summary>
    [Serializable]
    public class CrowbarException : Exception
    {
        internal CrowbarException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates a new instance of <see cref="CrowbarException"/>.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected CrowbarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// The stack trace of the captured exception.
        /// </summary>
        public override string StackTrace
        {
            get { return InnerException.StackTrace; }
        }
    }
}