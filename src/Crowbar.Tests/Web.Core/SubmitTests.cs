using System.Collections.Generic;
using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class SubmitTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_form_with_anti_forgery_request_token_using_render()
        {
            Application.Execute(browser =>
            {
                const string username = "crowbar";

                var context = new PartialViewContext("~/Views/_FormAntiForgeryRequestToken.cshtml").SetFormsAuthPrincipal(username);
                var payload = new TextBoxPayload { Text = "text" };

                var response = browser.Render(context, payload).Submit(ctx => ctx.FormsAuth(username));

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_be_able_to_post_form_with_checkbox_using_render(bool condition)
        {
            Application.Execute(browser =>
            {
                var payload = new CheckBoxPayload { Condition = condition, SanityCheck = condition.ToString() };
                var response = browser.Render("~/Views/_FormCheckBox.cshtml", payload).Submit();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_be_able_to_post_form_with_checkbox_using_load(bool condition)
        {
            Application.Execute(browser =>
            {
                var payload = new CheckBoxPayload { Condition = condition, SanityCheck = condition.ToString() };
                var response = browser.Load(CrowbarRoute.SubmitCheckBox.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("input[type=\"text\"]").Val(model.SanityCheck);
                    form.Find("input[type=\"checkbox\"]").Get(0).Checked = model.Condition;
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textbox_using_render()
        {
            Application.Execute(browser =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Render("~/Views/_FormTextBox.cshtml", payload).Submit();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textbox_using_load()
        {
            Application.Execute(browser =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Load(CrowbarRoute.SubmitTextBox.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("input[type=\"text\"]").Val(model.Text);
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textarea_using_render()
        {
            Application.Execute(browser =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Render("~/Views/_FormTextArea.cshtml", payload).Submit();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textarea_using_load()
        {
            Application.Execute(browser =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = browser.Load(CrowbarRoute.SubmitTextArea.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("textarea").Text(model.Text);
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_drop_down_using_render()
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

                var response = browser.Render("~/Views/_FormDropDown.cshtml", payload).Submit();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_drop_down_using_load()
        {
            Application.Execute(browser =>
            {
                var payload = new DropDownPayload
                {
                    Value = "2"
                };

                var response = browser.Load(CrowbarRoute.SubmitDropDown.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("select").Val(model.Value);
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}