using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class TestController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.Root)]
        public ActionResult Index_Delete(string id)
        {
            using (var session = Store.OpenSession())
            {
                var model = session.Load<Model>(id);
                session.Delete(model);
                session.SaveChanges();
            }

            return HttpOk();
        }

        [GET(CrowbarRoute.Root)]
        public ActionResult Index_Get(string id)
        {
            using (var session = Store.OpenSession())
            {
                var model = session.Load<Model>(id);
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

        [PUT(CrowbarRoute.Root)]
        public ActionResult Index_Put(string text)
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