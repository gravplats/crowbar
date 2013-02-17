using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting.Web.Mvc;

namespace Tool.Web
{
    [Authorize, RequireHttps]
    public class ToolController : Controller
    {
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
            if (form.Username == "admin" && form.Password == "admin")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return Redirect("/app");
            }

            return Redirect("/");
        }
    }
}