using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class AjaxTestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Delete()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [GET(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Get()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [POST(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Post()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [PUT(CrowbarRoute.Ajax), AjaxOnly]
        public ActionResult Ajax_Put()
        {
            return Assert(() => Request.IsAjaxRequest());
        }
    }
}