using System;
using System.Runtime.Serialization;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class ThrowsController : CrowbarControllerBase
    {
        public class NonSerializableCrowbarException : Exception
        {
            public NonSerializableCrowbarException(string message) : base(message) { }
        }

        [Serializable]
        public class SerializableCrowbarException : Exception
        {
            public SerializableCrowbarException(string message) : base(message) { }

            protected SerializableCrowbarException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        }

        [POST(CrowbarRoute.ExceptionNonSerializable)]
        public ActionResult NonSerializableException()
        {
            throw new NonSerializableCrowbarException("This is a non-serializable exception.");
        }

        [POST(CrowbarRoute.ExceptionSerializable)]
        public ActionResult SerializableException()
        {
            throw new SerializableCrowbarException("This is a serializable exception.");
        }
    }
}