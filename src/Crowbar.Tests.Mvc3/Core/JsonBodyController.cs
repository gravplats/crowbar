using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class JsonBodyController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.JsonBody)]
        public ActionResult JsonBody_Delete(string payload)
        {
            return Assert(() => payload == "text");
        }

        [POST(CrowbarRoute.JsonBody)]
        public ActionResult JsonBody_Post(string payload)
        {
            return Assert(() => payload == "text");
        }

        [PUT(CrowbarRoute.JsonBody)]
        public ActionResult JsonBody_Put(string payload)
        {
            return Assert(() => payload == "text");
        }
    }
}