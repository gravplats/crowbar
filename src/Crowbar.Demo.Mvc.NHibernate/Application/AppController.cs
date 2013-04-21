using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using Crowbar.Demo.Mvc.NHibernate.Application.Models;
using NHibernate;

namespace Crowbar.Demo.Mvc.NHibernate.Application
{
    [Authorize]
    [RequireHttps]
    public class AppController : Controller
    {
        protected ISession NHibernateSession
        {
            get { return (ISession)HttpContext.Items[Application.App.NHibernateSessionKey]; }
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
            var user = NHibernateSession.QueryOver<User>().Where(x => x.Username == form.Username).SingleOrDefault();
            if (user != null && user.Password.IsValid(form.Password))
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return Redirect(AppRoute.App);
            }

            return Redirect(AppRoute.Root);
        }
    }
}