using System.Web.Mvc;

namespace Crowbar.Mvc.Core
{
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(200);
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