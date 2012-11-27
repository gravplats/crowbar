using System.Web.Mvc;
using System.Xml.Serialization;

namespace Crowbar.Web
{
    public class XmlResult<TModel> : ActionResult
    {
        private readonly TModel model;

        public XmlResult(TModel model)
        {
            this.model = model;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/xml";

            var serializer = new XmlSerializer(typeof(TModel));
            serializer.Serialize(response.Output, model);
        }
    }
}