using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class HttpsTestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.Secure), RequireHttps]
        public ActionResult Secure_Delete()
        {
            return HttpOk();
        }

        [GET(CrowbarRoute.Secure), RequireHttps]
        public ActionResult Secure_Get()
        {
            return HttpOk();
        }

        [POST(CrowbarRoute.Secure), RequireHttps]
        public ActionResult Secure_Post()
        {
            return HttpOk();
        }

        [PUT(CrowbarRoute.Secure), RequireHttps]
        public ActionResult Secure_Put()
        {
            return HttpOk();
        }
    }
}