using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class ShouldBeTests : TestBase
    {
        [Test]
        public void Should_be_able_to_receive_html()
        {
            Execute(client =>
            {
                var response = client.Get(CrowbarRoute.ShouldBeHtml);
                response.ShouldBeHtml(document =>
                {
                    var div = document["#csquery"];
                    Assert.That(div, Has.Length.EqualTo(1));
                });
            });
        }

        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_receive_json(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, CrowbarRoute.ShouldBeJson);
                response.ShouldBeJson(json => Assert.That(json.payload, Is.EqualTo("text")));
            });
        }

        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_receive_xml(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, CrowbarRoute.ShouldBeXml);
                response.ShouldBeXml(xml => Assert.That(xml.Value, Is.EqualTo("text")));
            });
        }
    }
}