using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class FormSubmissionTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_form()
        {
            Application.Execute(browser =>
            {
                var payload = new Payload { Text = "text" };
                var response = browser.Submit("~/Views/FormSubmissionTest/Form.cshtml", payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_be_able_to_post_form_with_checkbox(bool condition)
        {
            Application.Execute(browser =>
            {
                var payload = new CheckBoxPayload { Condition = condition, SanityCheck = condition.ToString() };
                var response = browser.Submit("~/Views/FormSubmissionTest/FormCheckBox.cshtml", payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_anti_forgery_request_token()
        {
            Application.Execute(browser =>
            {
                const string username = "crowbar";

                var context = new PartialViewContext("~/Views/FormSubmissionTest/FormAntiForgeryRequestToken.cshtml");
                context.SetFormsAuthPrincipal(username);

                var payload = new Payload { Text = "text" };
                var response = browser.Submit(context, payload, ctx =>
                {
                    ctx.FormsAuth(username);
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}