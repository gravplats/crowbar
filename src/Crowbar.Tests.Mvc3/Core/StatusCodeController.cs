using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class StatusCodeController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.RedirectTarget)]
        public ActionResult RedirectTarget()
        {
            return HttpOk();
        }

        [GET(CrowbarRoute.RedirectPermanent)]
        public ActionResult RedirectPermanent()
        {
            return RedirectToActionPermanent("RedirectTarget");
        }

        [GET(CrowbarRoute.RedirectTemporary)]
        public ActionResult RedirectTemporary()
        {
            return RedirectToAction("RedirectTarget");
        }
    }
}