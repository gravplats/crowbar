using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class StatusCodeTests : TestBase
    {
        [Test]
        public void Should_be_able_to_perform_permanent_redirect()
        {
            Execute(client =>
            {
                var response = client.Get(CrowbarRoute.RedirectPermanent);
                response.ShouldHavePermanentlyRedirectTo(CrowbarRoute.RedirectTarget);
            });
        }

        [Test]
        public void Should_be_able_to_perform_temporary_redirect()
        {
            Execute(client =>
            {
                var response = client.Get(CrowbarRoute.RedirectTemporary);
                response.ShouldHaveTemporarilyRedirectTo(CrowbarRoute.RedirectTarget);
            });
        }

        [Test]
        public void Should_return_http_not_found_when_performing_get_against_an_unknown_path()
        {
            Execute(client =>
            {
                var response = client.Get("/unknown");
                response.ShouldHaveStatusCode(HttpStatusCode.NotFound);
            });
        }

        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_return_http_method_not_allowed_when_performing_non_get_against_an_unknown_path(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, "/unknown");
                response.ShouldHaveStatusCode(HttpStatusCode.MethodNotAllowed);
            });
        }
    }
}