using System.Web.Mvc;
using System.Web.Security;
using Crowbar.Demo.Mvc.Application;
using Crowbar.Demo.Mvc.Application.Models;
using NUnit.Framework;

namespace Crowbar.Demo.Mvc.Tests
{
    public class As_a_user_requesting_the_login_page : UserStory
    {
        [Then("should receive the login page")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                // Act.
                var response = client.Get(AppRoute.Root);

                // Assert.
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
            Application.Execute(client =>
            {
                // Arrange.
                const string Username = "admin";

                var form = new LoginForm
                {
                    Username = Username,
                    Password = "admin"
                };

                var view = new CrowbarViewContext("_LoginForm").SetAnonymousPrincipal();

                // Act.
                var response = client.Render(view, form).Submit();

                // Assert.
                response.ShouldHaveTemporarilyRedirectTo(AppRoute.App);
                response.ShouldHaveCookie(FormsAuthentication.FormsCookieName);
            });
        }
    }

    public class As_a_user_logging_in_with_correct_credentials_but_invalid_antiforgery_request_token : UserStory
    {
        [Then("should not be granted access to the application")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                // Arrange.
                const string Username = "admin";
                var form = new LoginForm
                {
                    Username = Username,
                    Password = "admin"
                };

                var view = new CrowbarViewContext("_LoginForm");
                view.SetFormsAuthPrincipal("invalid"); // simulate invalid anti-forgery request token.

                // Act.
                // Obviously the MVC application should handle this more gracefully, this is just an example.
                var exception = Assert.Throws<CrowbarException>(() => client.Render(view, form).Submit());

                // Assert.
                Assert.That(exception.InnerException, Is.TypeOf<HttpAntiForgeryException>());
            });
        }
    }

    public class As_a_user_logging_in_with_incorrect_credentials : UserStory
    {
        [Then("should be redirected to the login page")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                // Arrange.
                var form = new LoginForm
                {
                    Username = "incorrect",
                    Password = "incorrect"
                };

                // Act.
                var response = client.Render("_LoginForm", form).Submit();

                // Assert.
                response.ShouldHaveTemporarilyRedirectTo(AppRoute.Root);
            });
        }
    }

    public class As_an_authenticated_user_requesting_the_application_page : UserStory
    {
        [Then("should receive the application page")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                // Act.
                var response = client.Get(AppRoute.App, payload => payload.FormsAuth("admin"));

                // Assert.
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
            Application.Execute(client =>
            {
                // Act.
                var response = client.Get(AppRoute.App);

                // Assert.
                response.ShouldHaveTemporarilyRedirectTo("/?ReturnUrl=%2fapp");
            });
        }
    }
}
