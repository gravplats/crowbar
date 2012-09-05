using System.Linq;
using System.Web.Mvc;
using Raven.Client;

namespace Crowbar.Mvc.Core
{
    public class Model
    {
        public string Text { get; set; }
    }

    public class TestController : Controller
    {
        public static IDocumentStore Store;

        [HttpGet]
        public ActionResult Index()
        {
            using (var session = Store.OpenSession())
            {
                var models = session.Query<Model>().Customize(x => x.WaitForNonStaleResults()).ToList();
                if (models.Any())
                {
                    return new HttpStatusCodeResult(200);
                }
            }

            return new HttpStatusCodeResult(400);
        }

        [HttpPost]
        public ActionResult Index(string text)
        {
            if (text == null)
            {
                return new HttpStatusCodeResult(400);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}