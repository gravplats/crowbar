using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Mvc.Core
{
    public class FormsAuthTestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.Authentication), Authorize]
        public ActionResult Auth_Delete()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [GET(CrowbarRoute.Authentication), Authorize]
        public ActionResult Auth_Get()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [POST(CrowbarRoute.Authentication), Authorize]
        public ActionResult Auth_Post()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [PUT(CrowbarRoute.Authentication), Authorize]
        public ActionResult Auth_Put()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }
    }
}