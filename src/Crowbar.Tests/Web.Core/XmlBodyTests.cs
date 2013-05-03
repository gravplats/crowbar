using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class XmlBodyTests : TestBase
    {
        [TestCase("DELETE")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_be_able_to_send_xml(string method)
        {
            Execute(client =>
            {
                var response = client.PerformRequest(method, CrowbarRoute.XmlBody, x => x.XmlBody(new Payload{ Text = "text" }));
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });            
        }
    }
}