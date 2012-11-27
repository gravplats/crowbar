using System;
using System.Web.Mvc;
using Raven.Client;

namespace Crowbar.Web
{
    public abstract class CrowbarControllerBase : Controller
    {
        public static IDocumentStore Store;

        protected ActionResult Assert(Func<bool> assertion)
        {
            return assertion() ? HttpOk() : HttpBadRequest();
        }

        protected ActionResult HttpBadRequest()
        {
            return new HttpStatusCodeResult(400);
        }

        protected ActionResult HttpOk()
        {
            return new HttpStatusCodeResult(200);
        }

        protected ActionResult Xml<TModel>(TModel model)
        {
            return new XmlResult<TModel>(model);
        }
    }
}