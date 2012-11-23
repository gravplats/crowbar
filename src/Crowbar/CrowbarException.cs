using System;
using System.Runtime.Serialization;

namespace Crowbar
{
    /// <summary>
    /// Exception that is thrown by assert extensions.
    /// </summary>
    [Serializable]
    public class CrowbarException : Exception
    {
        public CrowbarException() { }

        public CrowbarException(string message) : base(message) { }

        public CrowbarException(string message, Exception innerException) : base(message, innerException) { }

        protected CrowbarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string StackTrace
        {
            get { return InnerException.StackTrace; }
        }
    }
}