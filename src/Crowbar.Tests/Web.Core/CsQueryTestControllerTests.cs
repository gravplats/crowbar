using Crowbar.Web;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class CsQueryTestControllerTests : TestBase
    {
        [Test]
        public void Should_return_html()
        {
            Application.Execute((browser, _) =>
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