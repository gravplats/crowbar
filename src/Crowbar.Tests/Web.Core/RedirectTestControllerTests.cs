using Crowbar.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class RedirectTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_perform_permanent_redirect()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get(CrowbarRoute.RedirectPermanent);
                response.ShouldHavePermanentlyRedirectTo(CrowbarRoute.Redirected);
            });
        }

        [Test]
        public void Should_be_able_to_perform_temporary_redirect()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get(CrowbarRoute.RedirectTemporary);
                response.ShouldHaveTemporarilyRedirectTo(CrowbarRoute.Redirected);
            });
        }
    }
}