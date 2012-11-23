using System;
using Crowbar.Web;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class ThrowsControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_handle_non_serializable_exceptions()
        {
            Application.Execute((browser, _) =>
            {
                Assert.Throws<Exception>(() => browser.Post(CrowbarRoute.ExceptionNonSerializable));
            });
        }

        [Test]
        public void Should_be_able_to_handle_serializable_exceptions()
        {
            Application.Execute((browser, _) =>
            {
                Assert.Throws<CrowbarException>(() => browser.Post(CrowbarRoute.ExceptionSerializable));
            });
        }
    }
}