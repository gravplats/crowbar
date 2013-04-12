using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Demo.Mvc.Async.Application
{
    public class AppController : Controller
    {
        private readonly IExternalRequestAsync request;

        public AppController(IExternalRequestAsync request)
        {
            this.request = request;
        }

        [GET(AppRoute.External)]
        public async Task<ActionResult> GetCrowbars()
        {
            var container = await request.Execute();
            return Json(container, JsonRequestBehavior.AllowGet);
        }
    }
}