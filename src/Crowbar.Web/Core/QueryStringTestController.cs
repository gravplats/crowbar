using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class QueryStringTestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.Query)]
        public ActionResult QueryString(string withMethod, string withPath)
        {
            return Assert(() => withMethod == "CrowbarWithMethod" && withPath == "CrowbarWithPath");
        }
    }
}