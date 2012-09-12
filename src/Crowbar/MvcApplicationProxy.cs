using System;
using System.Web;

namespace Crowbar
{
    public class MvcApplicationProxy : MarshalByRefObject, IMvcApplicationProxy
    {
        public void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory)
        {
            initialize.Delegate();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Process(SerializableDelegate<Action<Browser>> script)
        {
            script.Delegate(new Browser());
        }
    }
}