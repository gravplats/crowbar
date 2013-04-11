using System.Web;
using NUnit.Framework;

namespace Crowbar.Tests
{
    public class CrowbarControllerTests : TestBase
    {
        [TestCase("_ToString")]
        [TestCase("~/Views/Partials/_ToString.cshtml")]
        public void Can_render_partial_view_to_string(string viewName)
        {
            Application.Execute(client =>
            {
                HttpCookieCollection cookies;
                Assert.DoesNotThrow(() => CrowbarController.ToString(viewName, null, out cookies));
            });
        }

        [TestCase("ToString")]
        [TestCase("~/Views/ToString.cshtml")]
        public void Can_render_view_to_string(string viewName)
        {
            Application.Execute(client =>
            {
                HttpCookieCollection cookies;
                Assert.DoesNotThrow(() => CrowbarController.ToString(viewName, null, out cookies));
            });
        }

        [Test]
        public void Can_specify_custom_route_data_values()
        {
            Application.Execute(client =>
            {
                HttpCookieCollection cookies;

                var context = new CrowbarViewContext("Index") { ControllerName = "Custom" };
                Assert.DoesNotThrow(() => CrowbarController.ToString(context, null, out cookies));
            });
        }
    }
}