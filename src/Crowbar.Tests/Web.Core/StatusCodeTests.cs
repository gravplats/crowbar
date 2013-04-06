using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class StatusCodeTests : TestBase
    {
        [Test]
        public void Should_be_able_to_perform_permanent_redirect()
        {
            Application.Execute(client =>
            {
                var response = client.Get(CrowbarRoute.RedirectPermanent);
                response.ShouldHavePermanentlyRedirectTo(CrowbarRoute.RedirectTarget);
            });
        }

        [Test]
        public void Should_be_able_to_perform_temporary_redirect()
        {
            Application.Execute(client =>
            {
                var response = client.Get(CrowbarRoute.RedirectTemporary);
                response.ShouldHaveTemporarilyRedirectTo(CrowbarRoute.RedirectTarget);
            });
        }

        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_return_http_not_found_when_performing_request_against_an_unknown_path(string method)
        {
            Application.Execute(client =>
            {
                var response = client.PerformRequest(method, "/unknown");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }
    }
}