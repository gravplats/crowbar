using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class HeaderController : CrowbarControllerBase
    {
         [GET(CrowbarRoute.Header)]
         public ActionResult Headers_Get()
         {
             Response.AddHeader("X-Crowbar", "crowbar");
             return HttpOk();
         }
    }
}