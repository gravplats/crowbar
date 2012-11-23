using System.Web.Mvc;
using System.Xml.Serialization;

namespace Crowbar.Web.Core
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
}