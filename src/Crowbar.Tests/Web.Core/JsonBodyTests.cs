using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class JsonBodyTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_send_json(string method)
        {
            Application.Execute(browser =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.JsonBody, ctx => ctx.JsonBody(new { payload = "text" }));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}