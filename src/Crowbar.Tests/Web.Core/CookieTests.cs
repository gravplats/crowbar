using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class CookieTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_set_custom_cookie(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, CrowbarRoute.Cookie);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                response.ShouldHaveCookie("CustomCookie", "crowbar");
            });
        }
    }
}