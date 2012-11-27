using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class AntiForgeryRequestTokenTestController : CrowbarControllerBase
    {
        public const string Salt = "crowbar";

        [DELETE(CrowbarRoute.AntiForgeryToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryToken_Delete()
        {
            return HttpOk();
        }

        [POST(CrowbarRoute.AntiForgeryToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryToken_Post()
        {
            return HttpOk();
        }

        [PUT(CrowbarRoute.AntiForgeryToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryToken_Put()
        {
            return HttpOk();
        }

        // Using a salt is deprecated in ASP.NET MVC 4: http://stackoverflow.com/questions/10851283/antiforgerytoken-deprecated-in-asp-net-mvc-4-rc
    }
}