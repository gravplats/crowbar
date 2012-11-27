using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class CsQueryTestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.CsQuery)]
        public ActionResult Index()
        {
            return View("CsQuery");
        }
    }
}