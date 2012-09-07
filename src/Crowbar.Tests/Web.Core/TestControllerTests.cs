using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class TestControllerTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_return_http_not_found_when_performing_request_against_an_unknown_path(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, "/unknown");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }
    }
}