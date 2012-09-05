using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests
{
    public class QueryStringTestControllerTests : TestBase
    {
        [Test]
        public void Can_handle_both_query_string_with_method_and_with_path()
        {
            Server.Execute((context, browser) =>
            {
                string path = CrowbarRoute.Query.AsOutbound(new { withPath = "CrowbarWithPath" });
                var response = browser.Get(path, ctx => ctx.Query("withMethod", "CrowbarWithMethod"));

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}