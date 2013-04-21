using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class XmlBodyController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.XmlBody)]
        public ActionResult XmlBody_Delete([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [POST(CrowbarRoute.XmlBody)]
        public ActionResult XmlBody_Post([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [PUT(CrowbarRoute.XmlBody)]
        public ActionResult XmlBody_Put([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }
    }
}