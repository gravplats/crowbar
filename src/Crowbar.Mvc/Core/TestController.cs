using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Mvc.Core
{
    public class TestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.Root)]
        public ActionResult Index_Get(string id)
        {
            using (var session = Store.OpenSession())
            {
                var model = session.Load<Model>("models/" + id);
                return model != null ? HttpOk() : HttpBadRequest();
            }
        }

        [POST(CrowbarRoute.Root)]
        public ActionResult Index_Post(string text)
        {
            using (var session = Store.OpenSession())
            {
                var model = new Model { Text = text };
                session.Store(model);
                session.SaveChanges();

                return Json(new { id = model.Id });
            }
        }
    }
}