using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class CookieController : CrowbarControllerBase
    {
        private ActionResult Cookies()
        {
            Response.AppendCookie(new HttpCookie("Crowbar1", "Pry Bar"));
            Response.AppendCookie(new HttpCookie("Crowbar2", "Wrecking Bar"));
            Response.AppendCookie(new HttpCookie("Crowbar3", "Digging Bar"));

            return HttpOk();
        }

        [DELETE(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Delete()
        {
            return Cookies();
        }

        [GET(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Get()
        {
            return Cookies();
        }

        [POST(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Post()
        {
            return Cookies();
        }

        [PUT(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Put()
        {
            return Cookies();
        }
    }
}