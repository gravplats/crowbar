using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class RedirectTestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.Redirected)]
        public ActionResult Redirected()
        {
            return HttpOk();
        }

        [GET(CrowbarRoute.RedirectPermanent)]
        public ActionResult RedirectPermanent()
        {
            return RedirectToActionPermanent("Redirected");
        }

        [GET(CrowbarRoute.RedirectTemporary)]
        public ActionResult RedirectTemporary()
        {
            return RedirectToAction("Redirected");
        }
    }
}