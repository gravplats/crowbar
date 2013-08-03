using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class MustacheController : CrowbarControllerBase
    {
        [POST(CrowbarRoute.Mustache)]
        public ActionResult Mustache(Payload payload)
        {
            return Assert(() => payload.Text == "Mustache");
        }
    }
}