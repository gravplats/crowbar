using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class QueryStringTests : TestBase
    {
        [Test]
        public void Can_handle_both_query_string_with_method_and_with_path()
        {
            Execute(client =>
            {
                string path = CrowbarRoute.QueryString.AsOutbound(new { withPath = "CrowbarWithPath" });
                var response = client.Get(path, x => x.QueryString("withMethod", "CrowbarWithMethod"));

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}