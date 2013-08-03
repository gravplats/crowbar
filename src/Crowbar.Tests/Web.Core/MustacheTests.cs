using Crowbar.Mustache;
using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    [TestFixture]
    public class MustacheTests : TestBase
    {
        [Test]
        public void Should_be_able_to_post_a_mustache_template()
        {
            Execute(browser =>
            {
                var payload = new Payload { Text = "Mustache" };
                // Piggybacking on Razor (*.cshtml) so that we don't have to argue with build providers.
                var response = browser.Mustache("~/Views/Templates/Form.cshtml", payload).Submit();

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }

        [Test]
        public void Should_be_able_to_post_a_mustache_template_using_ajax()
        {
            Execute(browser =>
            {
                var payload = new Payload { Text = "Mustache" };
                // Piggybacking on Razor (*.cshtml) so that we don't have to argue with build providers.
                var response = browser.Mustache("~/Views/Templates/Form.cshtml", payload).AjaxSubmit();

                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }
    }
}