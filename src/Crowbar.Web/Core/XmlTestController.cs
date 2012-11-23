using System.Web.Mvc;
using System.Xml.Serialization;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class XmlTestController : CrowbarControllerBase
    {
        public class XmlModelBinder : CustomModelBinderAttribute, IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var serializer = new XmlSerializer(bindingContext.ModelType);
                return serializer.Deserialize(controllerContext.HttpContext.Request.InputStream);
            }

            public override IModelBinder GetBinder()
            {
                return this;
            }
        }

        public class Payload
        {
            public string Text { get; set; }
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