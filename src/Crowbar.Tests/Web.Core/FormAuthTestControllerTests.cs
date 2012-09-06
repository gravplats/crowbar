using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class FormAuthTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_a_request_using_forms_authentication(string method)
        {
            Application.Execute((context, browser) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.Authentication, ctx => ctx.FormsAuth("crowbar"));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}