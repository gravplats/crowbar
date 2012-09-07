using System.Web;

namespace Crowbar
{
    public class MvcApplicationProxy : MvcApplicationProxyBase<object>
    {
        protected override object CreateContext(HttpApplication application)
        {
            return new object();
        }
    }
}