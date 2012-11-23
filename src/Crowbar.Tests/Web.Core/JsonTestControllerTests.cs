using Crowbar.Web;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class JsonTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_receive_json(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.JsonResponse);
                response.ShouldBeJson(json => Assert.That(json.payload, Is.EqualTo("text")));
            });
        }

        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_send_json(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.JsonRequest, ctx => ctx.JsonBody(new { payload = "text" }));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}