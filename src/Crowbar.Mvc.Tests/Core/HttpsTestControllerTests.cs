using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class HttpsTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_an_https_request(string method)
        {
            Server.Execute((_, browser) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.Secure, ctx => ctx.HttpsRequest());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}