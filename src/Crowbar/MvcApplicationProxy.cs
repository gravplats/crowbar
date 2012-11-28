using System;
using System.Web;

namespace Crowbar
{
    public class MvcApplicationProxy : ProxyBase
    {
        public override void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory)
        {
            initialize.Delegate();
        }

        public void Process(SerializableDelegate<Action<Browser>> script)
        {
            try
            {
                var browser = CreateBrowser();
                script.Delegate(browser);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}