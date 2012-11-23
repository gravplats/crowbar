using System;
using System.Web;

namespace Crowbar
{
    public abstract class ProxyBase : MarshalByRefObject
    {
        public abstract void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory);

        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected void HandleException(Exception exception)
        {
            if (exception.IsSerializable())
            {
                throw new CrowbarException("An exception was thrown during the execution of the test.", exception);
            }

            throw new Exception(string.Format("An exception was thrown during the execution of the test: {0}", exception.Message));
        }
    }
}