using System.Web;
using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Core
{
    public class CookieTestControllerTests : TestBase
    {
        [Test]
        public void Can_captuare_cookies()
        {
            Application.Execute((_, browser) =>
            {
                var response = browser.Post(CrowbarRoute.Authorize);
                HttpResponse httpResponse = response.Response;
                HttpCookieCollection httpCookieCollection = httpResponse.Cookies;
            });
        }
    }
}