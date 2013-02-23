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
            Application.Execute(browser =>
            {
                HttpCookieCollection cookies;
                Assert.DoesNotThrow(() => CrowbarController.ToString(viewName, null, out cookies));
            });
        }

        [TestCase("ToString")]
        [TestCase("~/Views/ToString.cshtml")]
        public void Can_render_view_to_string(string viewName)
        {
            Application.Execute(browser =>
            {
                HttpCookieCollection cookies;
                Assert.DoesNotThrow(() => CrowbarController.ToString(viewName, null, out cookies));
            });            
        }
    }
}