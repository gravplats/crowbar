using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class RedirectTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_perform_permanent_redirect()
        {
            Server.Execute((_, browser) =>
            {
                var response = browser.Get(CrowbarRoute.RedirectPermanent);
                response.ShouldHavePermanentlyRedirectTo(CrowbarRoute.Redirected);
            });
        }

        [Test]
        public void Should_be_able_to_perform_temporary_redirect()
        {
            Server.Execute((_, browser) =>
            {
                var response = browser.Get(CrowbarRoute.RedirectTemporary);
                response.ShouldHaveTemporarilyRedirectTo(CrowbarRoute.Redirected);
            });
        }
    }
}