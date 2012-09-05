using System.Web;
using System.Web.Helpers;
using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests
{
    public class TestControllerTests
    {
        // Creating the server is a time-consuming process and should preferably only be done once.
        private readonly Server server = ServerFactory.Create("Crowbar.Mvc");

        [Test]
        public void Delete_Index()
        {
            server.Execute(session =>
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
            server.Execute(session =>
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
            server.Execute(session =>
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
            server.Execute(session =>
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