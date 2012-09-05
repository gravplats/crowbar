using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Core
{
    public class JsonTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_receive_json(string method)
        {
            Application.Execute((_, browser) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.JsonResponse);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                response.ShouldBeJson(json => Assert.That(json.payload, Is.EqualTo("text")));
            });
        }

        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_send_json(string method)
        {
            Application.Execute((_, browser) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.JsonRequest, ctx => ctx.JsonBody(new { payload = "text" }));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}