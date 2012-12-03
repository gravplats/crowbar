using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Crowbar;
using NUnit.Framework;
using Raven.Web;

namespace Raven.Tests
{
    public class As_a_user_requesting_the_login_page : UserStory
    {
        [Then("should receive the login page")]
        protected override void Test()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get("/");
                response.ShouldBeHtml(document =>
                {
                    var login = document["#login"];
                    Assert.That(login, Has.Length.EqualTo(1));
                });
            });
        }
    }

    public class As_a_user_logging_in_with_correct_credentials_and_valid_antiforgery_request_token : UserStory
    {
        [Then("should be redirected to the application page")]
        [Then("should have an authentication cookie")]
        protected override void Test()
        {
            Application.Execute((browser, context) =>
            {
                const string Username = "admin";
                const string Password = "admin";

                // Save user to database.
                context.User(Username, Password);

                var form = new LoginForm
                {
                    Username = Username,
                    Password = Password
                };

                var view = new PartialViewContext("_LoginForm").SetAnonymousPrincipal();
                var response = browser.Render(view, form).Submit();

                response.ShouldHaveTemporarilyRedirectTo("/app");
                Assert.That(response.HttpResponse.Cookies.AllKeys.Any(name => name == FormsAuthentication.FormsCookieName), Is.True);
            });
        }
    }

    public class As_a_user_logging_in_with_correct_credentials_but_invalid_antiforgery_request_token : UserStory
    {
        [Then("should not be granted access to the application")]
        protected override void Test()
        {
            Application.Execute((browser, context) =>
            {
                const string Username = "admin";
                const string Password = "admin";

                // Save user to database.
                context.User(Username, Password);

                var form = new LoginForm
                {
                    Username = Username,
                    Password = Password
                };

                var view = new PartialViewContext("_LoginForm");
                view.SetFormsAuthPrincipal("invalid"); // simulate invalid anti-forgery request token.

                // Obviously the MVC application should handle this more gracefully, this is just an example.
                var exception = Assert.Throws<CrowbarException>(() => browser.Render(view, form).Submit());
                Assert.That(exception.InnerException, Is.TypeOf<HttpAntiForgeryException>());
            });
        }
    }

    public class As_a_user_logging_in_with_incorrect_credentials : UserStory
    {
        [Then("should be redirected to the login page")]
        protected override void Test()
        {
            Application.Execute((browser, context) =>
            {
                var form = new LoginForm
                {
                    Username = "incorrect",
                    Password = "incorrect"
                };

                var response = browser.Render("_LoginForm", form).Submit();
                response.ShouldHaveTemporarilyRedirectTo("/");
            });
        }
    }

    public class As_an_authenticated_user_requesting_the_application_page : UserStory
    {
        [Then("should receive the application page")]
        protected override void Test()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get("/app", ctx => ctx.FormsAuth("admin"));
                response.ShouldBeHtml(document =>
                {
                    var login = document["#app"];
                    Assert.That(login, Has.Length.EqualTo(1));
                });
            });
        }
    }

    public class As_an_unauthenticated_user_requesting_the_application_page : UserStory
    {
        [Then("should be redirected to the login page")]
        protected override void Test()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get("/app");
                response.ShouldHaveTemporarilyRedirectTo("/?ReturnUrl=%2fapp");
            });
        }
    }
}
