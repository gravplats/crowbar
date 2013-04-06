using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using Crowbar.Demo.Mvc.Application.Models;

namespace Crowbar.Demo.Mvc.Application
{
    [Authorize]
    [RequireHttps]
    public class AppController : Controller
    {
        [GET(AppRoute.App)]
        public ActionResult App()
        {
            return View("App");
        }

        [AllowAnonymous]
        [GET(AppRoute.Root)]
        public ActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [POST(AppRoute.Login)]
        public ActionResult Login(LoginForm form)
        {
            if (form.Username == "admin" && form.Password == "admin")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return Redirect(AppRoute.App);
            }

            return Redirect(AppRoute.Root);
        }
    }
}