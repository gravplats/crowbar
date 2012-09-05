using Crowbar.Hosting;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests
{
    public class TestControllerTests
    {
        private AppHost host;

        [TestFixtureSetUp]
        public void Context()
        {
            host = AppHost.Simulate("Crowbar.Mvc");
        }

        [Test]
        public void Get_Index()
        {
            host.Start(session =>
            {
                var result = session.Get("");
                Assert.That(result.Response.StatusCode, Is.EqualTo(200));
            });
        }

        [Test]
        public void Post_Index()
        {
            host.Start(session =>
            {
                var result = session.Post("", new { text = "text" });
                Assert.That(result.Response.StatusCode, Is.EqualTo(200));
            });
        }
    }
}