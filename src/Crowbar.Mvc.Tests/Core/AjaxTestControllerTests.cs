using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class AjaxTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_an_ajax_request(string method)
        {
            Application.Execute((_, browser) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.Ajax, ctx => ctx.AjaxRequest());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}