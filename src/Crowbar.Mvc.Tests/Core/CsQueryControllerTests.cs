using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class CsQueryControllerTests : TestBase
    {
        [Test]
        public void Should_return_html()
        {
            Application.Execute((_, browser) =>
            {
                var response = browser.Get(CrowbarRoute.CsQuery);
                response.ShouldBeHtml(document =>
                {
                    var div = document["#csquery"];
                    Assert.That(div, Has.Length.EqualTo(1));
                });
            });
        }
    }
}