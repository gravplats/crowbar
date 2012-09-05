using System.Web;
using System.Web.Helpers;
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
        public void Delete_Index()
        {
            host.Start(session =>
            {
                var model = new Model { Text = "Crowbar" };
                using (var raven = session.Store.OpenSession())
                {
                    raven.Store(model);
                    raven.SaveChanges();
                }

                session.Delete(CrowbarRoute.Root, ctx => ctx.FormValue("id", model.Id));

                using (var raven = session.Store.OpenSession())
                {
                    var crowbar = raven.Load<Model>(model.Id);
                    Assert.That(crowbar, Is.Null);
                }
            });
        }

        [Test]
        public void Get_Index()
        {
            host.Start(session =>
            {
                var model = new Model { Text = "Crowbar" };
                using (var raven = session.Store.OpenSession())
                {
                    raven.Store(model);
                    raven.SaveChanges();
                }

                string url = CrowbarRoute.Root + "?id=" + HttpUtility.UrlEncode(model.Id);
                var result = session.Get(url);
                Assert.That(result.Response.StatusCode, Is.EqualTo(200));
            });
        }

        [Test]
        public void Post_Index()
        {
            host.Start(session =>
            {
                var result = session.Post(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));
                using (var raven = session.Store.OpenSession())
                {
                    dynamic json = Json.Decode(result.ResponseText);
                    string id = json.id;

                    var model = raven.Load<Model>(id);
                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }

        [Test]
        public void Put_Index()
        {
            host.Start(session =>
            {
                var result = session.Put(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));
                using (var raven = session.Store.OpenSession())
                {
                    dynamic json = Json.Decode(result.ResponseText);
                    string id = json.id;

                    var model = raven.Load<Model>(id);
                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }
    }
}