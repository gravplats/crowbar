using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class XmlTestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.XmlResponse)]
        public ActionResult XmlResponse_Delete()
        {
            return Xml(new Payload { Text = "text" });
        }

        [GET(CrowbarRoute.XmlResponse)]
        public ActionResult XmlResponse_Get()
        {
            return Xml(new Payload { Text = "text" });
        }

        [POST(CrowbarRoute.XmlResponse)]
        public ActionResult XmlResponse_Post()
        {
            return Xml(new Payload { Text = "text" });
        }

        [PUT(CrowbarRoute.XmlResponse)]
        public ActionResult XmlResponse_Put()
        {
            return Xml(new Payload { Text = "text" });
        }

        [DELETE(CrowbarRoute.XmlRequest)]
        public ActionResult XmlRequest_Delete([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [POST(CrowbarRoute.XmlRequest)]
        public ActionResult XmlRequest_Post([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [PUT(CrowbarRoute.XmlRequest)]
        public ActionResult XmlRequest_Put([XmlModelBinder]Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }
    }
}