using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Mvc.Core
{
    public class CsQueryController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.CsQuery)]
        public ActionResult Index()
        {
            return View("CsQuery");
        }
    }
}