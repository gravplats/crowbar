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
            server.Execute(context =>
            {
                // Arrange.
                var model = new Model { Text = "Crowbar" };
                using (var session = context.Store.OpenSession())
                {
                    session.Store(model);
                    session.SaveChanges();
                }

                // Act.
                var response = context.Delete(CrowbarRoute.Root, ctx => ctx.FormValue("id", model.Id));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                using (var session = context.Store.OpenSession())
                {
                    var crowbar = session.Load<Model>(model.Id);
                    Assert.That(crowbar, Is.Null);
                }
            });
        }

        [Test]
        public void Get_Index()
        {
            server.Execute(context =>
            {
                // Arrange.
                var model = new Model { Text = "Crowbar" };
                using (var raven = context.Store.OpenSession())
                {
                    raven.Store(model);
                    raven.SaveChanges();
                }

                // Act.
                string url = CrowbarRoute.Root + "?id=" + HttpUtility.UrlEncode(model.Id);
                var response = context.Get(url);

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Post_Index()
        {
            server.Execute(context =>
            {
                // Act.
                var response = context.Post(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Response.ContentType, Is.EqualTo("application/json"));

                using (var session = context.Store.OpenSession())
                {
                    dynamic json = Json.Decode(response.ResponseText);
                    var model = session.Load<Model>((string)json.id);

                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }

        [Test]
        public void Put_Index()
        {
            server.Execute(context =>
            {
                // Act.
                var response = context.Put(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Response.ContentType, Is.EqualTo("application/json"));

                using (var session = context.Store.OpenSession())
                {
                    dynamic json = Json.Decode(response.ResponseText);
                    var model = session.Load<Model>((string)json.id);

                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }
    }
}