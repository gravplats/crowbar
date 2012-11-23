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
        public AssertException(string message) : base(message) { }

        public AssertException(string message, Exception innerException) : base(message, innerException) { }

        protected AssertException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}