using System.Collections.Generic;
using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class SubmitTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_form_with_anti_forgery_request_token_using_render()
        {
            Execute(client =>
            {
                const string username = "crowbar";

                var context = new CrowbarViewContext("~/Views/Partials/_FormAntiForgeryRequestToken.cshtml").SetFormsAuthPrincipal(username);
                var payload = new TextBoxPayload { Text = "text" };

                var response = client.Render(context, payload).Submit(x => x.FormsAuth(username));
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_be_able_to_post_form_with_checkbox_using_render(bool condition)
        {
            Execute(client =>
            {
                var payload = new CheckBoxPayload { Condition = condition, SanityCheck = condition.ToString() };

                var response = client.Render("~/Views/Partials/_FormCheckBox.cshtml", payload).Submit();
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_be_able_to_post_form_with_checkbox_using_load(bool condition)
        {
            Execute(client =>
            {
                var payload = new CheckBoxPayload { Condition = condition, SanityCheck = condition.ToString() };
                var response = client.Load(CrowbarRoute.SubmitCheckBox.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("input[type=\"text\"]").Val(model.SanityCheck);
                    form.Find("input[type=\"checkbox\"]").Get(0).Checked = model.Condition;
                });

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textbox_using_render()
        {
            Execute(client =>
            {
                var payload = new TextBoxPayload { Text = "text" };

                var response = client.Render("~/Views/Partials/_FormTextBox.cshtml", payload).Submit();
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textbox_using_load()
        {
            Execute(client =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = client.Load(CrowbarRoute.SubmitTextBox.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("input[type=\"text\"]").Val(model.Text);
                });

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textarea_using_render()
        {
            Execute(client =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                
                var response = client.Render("~/Views/Partials/_FormTextArea.cshtml", payload).Submit();
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_textarea_using_load()
        {
            Execute(client =>
            {
                var payload = new TextBoxPayload { Text = "text" };
                var response = client.Load(CrowbarRoute.SubmitTextArea.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("textarea").Text(model.Text);
                });

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_drop_down_using_render()
        {
            Execute(client =>
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

                var response = client.Render("~/Views/Partials/_FormDropDown.cshtml", payload).Submit();
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_form_with_drop_down_using_load()
        {
            Execute(client =>
            {
                var payload = new DropDownPayload
                {
                    Value = "2"
                };

                var response = client.Load(CrowbarRoute.SubmitDropDown.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("select").Val(model.Value);
                });

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_select_form_with_selector_using_render()
        {
            Execute(client =>
            {
                var payload = new MultipleFormsPayload
                {
                    Form1 = "form1",
                    Form2 = "form2"
                };

                var response = client.Render("~/Views/Partials/_MultipleForms.cshtml", payload).Submit(selector: "form.js-form2");
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_select_form_with_selector_using_load()
        {
            Execute(client =>
            {
                var payload = new MultipleFormsPayload
                {
                    Form1 = "form1",
                    Form2 = "form2"
                };

                var response = client.Load(CrowbarRoute.MultipleForms.AsOutbound()).Submit(payload, overrides: (form, model) =>
                {
                    form.Find("input[type='hidden']").Val(payload.Form2);
                }, selector: "form.js-form2");

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }
    }
}