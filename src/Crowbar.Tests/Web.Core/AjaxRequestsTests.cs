using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class AjaxRequestsTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_perform_an_ajax_request(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, CrowbarRoute.AjaxRequest, x => x.AjaxRequest());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}