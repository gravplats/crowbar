using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using Crowbar.Demo.Mvc.Raven.Application.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Crowbar.Demo.Mvc.Raven.Application
{
    [Authorize, RequireHttps]
    public class AppController : Controller
    {
        protected IDocumentSession RavenSession
        {
            get { return (IDocumentSession)HttpContext.Items[Application.App.RavenSessionKey]; }
        }

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
            var user = RavenSession.Query<User>().Where(x => x.Username == form.Username).FirstOrDefault();
            if (user != null && user.Password.IsValid(form.Password))
            {
                FormsAuthentication.SetAuthCookie(form.Username, false);
                return Redirect(AppRoute.App);
            }

            return Redirect(AppRoute.Root);
        }
    }
}