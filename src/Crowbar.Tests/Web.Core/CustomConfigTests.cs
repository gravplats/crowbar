using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class CustomConfigTests : TestBase
    {
        [Test]
        public void Should_be_able_to_set_custom_configuration_file()
        {
            Execute(client =>
            {
                var response = client.Get(CrowbarRoute.CustomConfig);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}