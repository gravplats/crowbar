using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class HeaderControllerTests : TestBase
    {
        [Test]
        public void Should_be_able_to_set_headers()
        {
            Execute(client =>
            {
                var response = client.Get(CrowbarRoute.Header);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                response.ShouldHaveHeader("X-Crowbar", "crowbar");
            });
        }
    }
}