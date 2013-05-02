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
        /// <param name="args">An object array that contains zero or more object to format.</param>
        public AssertException(string message, params object[] args) : base(string.Format(message, args)) { }

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

        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="response">The client response.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <returns>An assert exception.</returns>
        public static AssertException Create(ClientResponse response, string message)
        {
            message = ExtendMessage(response, message);
            return new AssertException(message);
        }

        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="response">The client response.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="args">An object array that contains zero or more object to format.</param>
        /// <returns>An assert exception.</returns>
        public static AssertException Create(ClientResponse response, string message, params object[] args)
        {
            message = ExtendMessage(response, message);
            return new AssertException(message, args);
        }

        /// <summary>
        /// Creates an instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="response">The client response.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>An assert exception.</returns>
        public static AssertException Create(ClientResponse response, string message, Exception innerException)
        {
            message = ExtendMessage(response, message);
            return new AssertException(message, innerException);
        }

        private static string ExtendMessage(ClientResponse response, string message)
        {
            if (response != null)
            {
                message += Environment.NewLine + Environment.NewLine + response.RawHttpResponse;
            }

            return message;
        }
    }
}