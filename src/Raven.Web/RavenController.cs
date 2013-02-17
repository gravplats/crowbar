using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;

namespace Raven.Web
{
    [Authorize, RequireHttps]
    public class RavenController : Controller
    {
        public static IDocumentStore Store;

        [GET("/app")]
        public ActionResult App()
        {
            return View("App");
        }

        [GET("/"), AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [POST("/login"), AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginForm form)
        {
            using (var session = Store.OpenSession())
            {
                var user = session.Query<User>().Where(x => x.Username == form.Username).FirstOrDefault();
                if (user != null && user.Password.IsValid(form.Password))
                {
                    FormsAuthentication.SetAuthCookie(form.Username, false);
                    return Redirect("/app");
                }
            }

            return Redirect("/");
        }
    }
}