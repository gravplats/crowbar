using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class CustomConfigTestControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_set_custom_configuration_file()
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.Get(CrowbarRoute.CustomConfig);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}