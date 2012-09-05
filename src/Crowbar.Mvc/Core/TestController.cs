using System.Web.Mvc;
using Raven.Client;

namespace Crowbar.Mvc.Core
{
    public class Model
    {
        public string Id { get; set; }

        public string Text { get; set; }
    }

    public class TestController : Controller
    {
        public static IDocumentStore Store;

        private ActionResult HttpBadRequest()
        {
            return new HttpStatusCodeResult(200);
        }

        private ActionResult HttpOk()
        {
            return new HttpStatusCodeResult(200);
        }

        [HttpGet, ActionName("Index")]
        public ActionResult Index_Get(string id)
        {
            using (var session = Store.OpenSession())
            {
                var model = session.Load<Model>("models/" + id);
                return model != null ? HttpOk() : HttpBadRequest();
            }
        }

        [HttpPost, ActionName("Index")]
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