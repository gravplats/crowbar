using System.Web;
using System.Web.Helpers;
using Crowbar.Mvc.Core;
using NUnit.Framework;

namespace Crowbar.Mvc.Tests.Core
{
    public class TestControllerTests : TestBase
    {
        [Test]
        public void Delete_Index()
        {
            Application.Execute((context, browser) =>
            {
                // Arrange.
                var model = new Model { Text = "Crowbar" };
                using (var session = context.Store.OpenSession())
                {
                    session.Store(model);
                    session.SaveChanges();
                }

                // Act.
                var response = browser.Delete(CrowbarRoute.Root, ctx => ctx.FormValue("id", model.Id));

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
            Application.Execute((context, browser) =>
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
                var response = browser.Get(url);

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Post_Index()
        {
            Application.Execute((context, browser) =>
            {
                // Act.
                var response = browser.Post(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                dynamic json = response.ShouldBeJson();

                using (var session = context.Store.OpenSession())
                {
                    var model = session.Load<Model>((string)json.id);

                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }

        [Test]
        public void Put_Index()
        {
            Application.Execute((context, browser) =>
            {
                // Act.
                var response = browser.Put(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                dynamic json = response.ShouldBeJson();

                using (var session = context.Store.OpenSession())
                {
                    var model = session.Load<Model>((string)json.id);

                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }

        [TestCase("DELETE")]
        [TestCase("GET")]
        [TestCase("POST")]
        [TestCase("PUT")]
        public void Should_return_http_not_found_when_performing_request_against_an_unknown_path(string method)
        {
            Application.Execute((context, browser) =>
            {
                var response = browser.PerformRequest(method, "/unknown");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }
    }
}