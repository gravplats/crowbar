using System;

namespace Crowbar
{
    /// <summary>
    /// Exception that is thrown by assert extensions.
    /// </summary>
    public class AssertException : Exception
    {
        public AssertException() { }

        public AssertException(string message) : base(message) { }

        public AssertException(string message, Exception innerException) : base(message, innerException) { }
    }
}