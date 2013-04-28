using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class CookieController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Delete()
        {
            var cookie = new HttpCookie("CustomCookie", "crowbar");
            Response.AppendCookie(cookie);

            return HttpOk();
        }

        [GET(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Get()
        {
            var cookie = new HttpCookie("CustomCookie", "crowbar");
            Response.AppendCookie(cookie);

            return HttpOk();
        }

        [POST(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Post()
        {
            var cookie = new HttpCookie("CustomCookie", "crowbar");
            Response.AppendCookie(cookie);

            return HttpOk();
        }

        [PUT(CrowbarRoute.Cookie)]
        public ActionResult Cookie_Put()
        {
            var cookie = new HttpCookie("CustomCookie", "crowbar");
            Response.AppendCookie(cookie);

            return HttpOk();
        }
    }
}