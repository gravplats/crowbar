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

        protected CrowbarException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override string StackTrace
        {
            get { return InnerException.StackTrace; }
        }
    }
}