using Crowbar.Hosting;
using Crowbar.Mvc.Core;
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
                using (var ses = session.Store.OpenSession())
                {
                    var model = new Model { Text = "Crowbar" };
                    ses.Store(model);
                    ses.SaveChanges();
                }

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