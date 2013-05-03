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

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
                response.ShouldHaveCookie("Crowbar1", "Pry Bar");
                response.ShouldHaveCookie("Crowbar2", "Wrecking Bar");
                response.ShouldHaveCookie("Crowbar3", "Digging Bar");
            });
        }
    }
}