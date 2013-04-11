using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class HttpsRequestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.HttpsRequest), RequireHttps]
        public ActionResult HttpsRequest_Delete()
        {
            return HttpOk();
        }

        [GET(CrowbarRoute.HttpsRequest), RequireHttps]
        public ActionResult HttpsRequest_Get()
        {
            return HttpOk();
        }

        [POST(CrowbarRoute.HttpsRequest), RequireHttps]
        public ActionResult HttpsRequest_Post()
        {
            return HttpOk();
        }

        [PUT(CrowbarRoute.HttpsRequest), RequireHttps]
        public ActionResult HttpsRequest_Put()
        {
            return HttpOk();
        }
    }
}