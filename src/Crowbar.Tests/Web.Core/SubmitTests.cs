using System.Collections.Generic;
using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class SubmitTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_form_with_anti_forgery_request_token()
        {
            Application.Execute(browser =>
            {
                const string username = "crowbar";

                var context = new PartialViewContext("~/Views/_FormAntiForgeryRequestToken.cshtml");
                context.SetFormsAuthPrincipal(username);

                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Submit(context, payload, ctx => ctx.FormsAuth(username));

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
                var response = browser.Submit("~/Views/_FormCheckBox.cshtml", payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textbox()
        {
            Application.Execute(browser =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Submit("~/Views/_FormTextBox.cshtml", payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_drop_down()
        {
            Application.Execute(browser =>
            {
                var payload = new DropDownPayload
                {
                    Values = new List<DropDownPayload.DropDownItem>
                    {
                        new DropDownPayload.DropDownItem
                        {
                            Text = "black",
                            Value = "1"
                        },
                        new DropDownPayload.DropDownItem
                        {
                            Text = "white",
                            Value = "2",
                            Selected = true
                        },
                    }
                };

                var response = browser.Submit("~/Views/_FormDropDown.cshtml", payload);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}