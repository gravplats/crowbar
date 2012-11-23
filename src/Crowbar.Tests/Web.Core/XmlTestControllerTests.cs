using Crowbar.Web;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class XmlTestControllerTests : TestBase
    {
        public class Payload
        {
            public string Text { get; set; }
        }

        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_receive_xml(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.XmlResponse);
                response.ShouldBeXml(xml => Assert.That(xml.Value, Is.EqualTo("text")));
            });
        }

        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_send_xml(string method)
        {
            Application.Execute((browser, _) =>
            {
                var response = browser.PerformRequest(method, CrowbarRoute.XmlRequest, ctx => ctx.XmlBody(new Payload{ Text = "text" }));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });            
        }
    }
}