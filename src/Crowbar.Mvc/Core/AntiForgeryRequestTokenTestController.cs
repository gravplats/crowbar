using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Mvc.Core
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

        [DELETE(CrowbarRoute.AntiForgeryTokenSalt), ValidateAntiForgeryToken(Salt = Salt)]
        public ActionResult AntiForgeryTokenSalt_Delete()
        {
            return HttpOk();
        }

        [POST(CrowbarRoute.AntiForgeryTokenSalt), ValidateAntiForgeryToken(Salt = Salt)]
        public ActionResult AntiForgeryTokenSalt_Post()
        {
            return HttpOk();
        }

        [PUT(CrowbarRoute.AntiForgeryTokenSalt), ValidateAntiForgeryToken(Salt = Salt)]
        public ActionResult AntiForgeryTokenSalt_Put()
        {
            return HttpOk();
        }
    }
}