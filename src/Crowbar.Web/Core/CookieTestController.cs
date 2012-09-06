using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class CookieTestController : CrowbarControllerBase
    {
        [POST(CrowbarRoute.Authorize)]
        public ActionResult Authorize(string username)
        {
            FormsAuthentication.SetAuthCookie(username, true);
            return HttpOk();
        }
    }
}