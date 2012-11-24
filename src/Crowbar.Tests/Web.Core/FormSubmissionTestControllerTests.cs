using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class FormSubmissionTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_form()
        {
            Application.Execute((browser, _) =>
            {
                var payload = new FormSubmissionTestController.Payload { Text = "text" };
                browser.Submit("~/Views/FormSubmissionTest/Form.cshtml", payload);
            });
        }
    }
}