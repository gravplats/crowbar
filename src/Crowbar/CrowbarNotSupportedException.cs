using System;
using System.Runtime.Serialization;

namespace Crowbar
{
    [Serializable]
    public class CrowbarNotSupportedException : Exception
    {
        public CrowbarNotSupportedException(string message) : base(message) { }

        protected CrowbarNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}