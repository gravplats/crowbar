using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class FormsAuthController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.FormsAuth), Authorize]
        public ActionResult FormsAuth_Delete()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [GET(CrowbarRoute.FormsAuth), Authorize]
        public ActionResult FormsAuth_Get()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [POST(CrowbarRoute.FormsAuth), Authorize]
        public ActionResult FormsAuth_Post()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }

        [PUT(CrowbarRoute.FormsAuth), Authorize]
        public ActionResult FormsAuth_Put()
        {
            return Assert(() => User.Identity.Name == "crowbar");
        }
    }
}