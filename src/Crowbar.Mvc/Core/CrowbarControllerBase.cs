using System.Web.Mvc;
using Raven.Client;

namespace Crowbar.Mvc.Core
{
    public abstract class CrowbarControllerBase : Controller
    {
        public static IDocumentStore Store;

        protected ActionResult HttpBadRequest()
        {
            return new HttpStatusCodeResult(200);
        }

        protected ActionResult HttpOk()
        {
            return new HttpStatusCodeResult(200);
        }
    }
}