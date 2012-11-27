using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class JsonTestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.JsonResponse)]
        public ActionResult JsonResponse_Delete()
        {
            return Json(new { payload = "text" });
        }

        [GET(CrowbarRoute.JsonResponse)]
        public ActionResult JsonResponse_Get()
        {
            return Json(new { payload = "text" }, JsonRequestBehavior.AllowGet);
        }

        [POST(CrowbarRoute.JsonResponse)]
        public ActionResult JsonResponse_Post()
        {
            return Json(new { payload = "text" });
        }

        [PUT(CrowbarRoute.JsonResponse)]
        public ActionResult JsonResponse_Put()
        {
            return Json(new { payload = "text" });
        }

        [DELETE(CrowbarRoute.JsonRequest)]
        public ActionResult JsonRequest_Delete(string payload)
        {
            return Assert(() => payload == "text");
        }

        [POST(CrowbarRoute.JsonRequest)]
        public ActionResult JsonRequest_Post(string payload)
        {
            return Assert(() => payload == "text");
        }

        [PUT(CrowbarRoute.JsonRequest)]
        public ActionResult JsonRequest_Put(string payload)
        {
            return Assert(() => payload == "text");
        }
    }
}