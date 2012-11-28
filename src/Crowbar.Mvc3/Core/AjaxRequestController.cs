using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class AjaxRequestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.AjaxRequest), AjaxOnly]
        public ActionResult AjaxRequest_Delete()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [GET(CrowbarRoute.AjaxRequest), AjaxOnly]
        public ActionResult AjaxRequest_Get()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [POST(CrowbarRoute.AjaxRequest), AjaxOnly]
        public ActionResult AjaxRequest_Post()
        {
            return Assert(() => Request.IsAjaxRequest());
        }

        [PUT(CrowbarRoute.AjaxRequest), AjaxOnly]
        public ActionResult AjaxRequest_Put()
        {
            return Assert(() => Request.IsAjaxRequest());
        }
    }
}