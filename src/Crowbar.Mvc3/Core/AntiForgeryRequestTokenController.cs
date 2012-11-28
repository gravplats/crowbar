using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class AntiForgeryRequestTokenController : CrowbarControllerBase
    {
        [DELETE(CrowbarRoute.AntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryRequestToken_Delete()
        {
            return HttpOk();
        }

        [POST(CrowbarRoute.AntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryRequestToken_Post()
        {
            return HttpOk();
        }

        [PUT(CrowbarRoute.AntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult AntiForgeryRequestToken_Put()
        {
            return HttpOk();
        }

        // Using a salt is deprecated in ASP.NET MVC 4: http://stackoverflow.com/questions/10851283/antiforgerytoken-deprecated-in-asp-net-mvc-4-rc
    }
}