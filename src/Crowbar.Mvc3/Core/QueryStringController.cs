using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class QueryStringController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.QueryString)]
        public ActionResult QueryString(string withMethod, string withPath)
        {
            return Assert(() => withMethod == "CrowbarWithMethod" && withPath == "CrowbarWithPath");
        }
    }
}