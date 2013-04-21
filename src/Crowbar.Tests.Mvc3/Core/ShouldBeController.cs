using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class ShouldBeController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.ShouldBeHtml)]
        public ActionResult ShouldBeHtml()
        {
            return View("ShouldBeHtml");
        }

        [DELETE(CrowbarRoute.ShouldBeJson)]
        public ActionResult ShouldBeJson_Delete()
        {
            return Json(new { payload = "text" });
        }

        [GET(CrowbarRoute.ShouldBeJson)]
        public ActionResult ShouldBeJson_Get()
        {
            return Json(new { payload = "text" }, JsonRequestBehavior.AllowGet);
        }

        [POST(CrowbarRoute.ShouldBeJson)]
        public ActionResult ShouldBeJson_Post()
        {
            return Json(new { payload = "text" });
        }

        [PUT(CrowbarRoute.ShouldBeJson)]
        public ActionResult ShouldBeJson_Put()
        {
            return Json(new { payload = "text" });
        }

        [DELETE(CrowbarRoute.ShouldBeXml)]
        public ActionResult ShouldBeXml_Delete()
        {
            return Xml(new Payload { Text = "text" });
        }

        [GET(CrowbarRoute.ShouldBeXml)]
        public ActionResult ShouldBeXml_Get()
        {
            return Xml(new Payload { Text = "text" });
        }

        [POST(CrowbarRoute.ShouldBeXml)]
        public ActionResult ShouldBeXml_Post()
        {
            return Xml(new Payload { Text = "text" });
        }

        [PUT(CrowbarRoute.ShouldBeXml)]
        public ActionResult ShouldBeXml_Put()
        {
            return Xml(new Payload { Text = "text" });
        }
    }
}