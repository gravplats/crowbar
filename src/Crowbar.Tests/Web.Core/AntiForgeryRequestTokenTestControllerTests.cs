using Crowbar.Web;
using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class AntiForgeryRequestTokenTestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_a_request_with_an_anti_forgery_token(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.AntiForgeryToken, ctx => ctx.AntiForgeryRequestToken());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_a_request_with_an_anti_forgery_token_with_username(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.AntiForgeryToken, ctx =>
                {
                    const string username = "crowbar";

                    ctx.FormsAuth(username);
                    ctx.AntiForgeryRequestToken(username);
                });

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}