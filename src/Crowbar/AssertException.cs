using System;
using System.Runtime.Serialization;

namespace Crowbar
{
    /// <summary>
    /// Exception that is thrown by assert extensions.
    /// </summary>
    [Serializable]
    public class AssertException : Exception
    {
        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AssertException(string message) : base(message) { }

        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public AssertException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}