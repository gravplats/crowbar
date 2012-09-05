using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class CustomConfigTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_set_custom_configuration_file()
        {
            Server.Execute((_, browser) =>
            {
                var response = browser.Get(CrowbarRoute.CustomConfig);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}