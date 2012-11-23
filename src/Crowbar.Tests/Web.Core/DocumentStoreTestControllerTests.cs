using System.Web;
using Crowbar.Web;
using Crowbar.Web.Domain;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class DocumentStoreTestControllerTests : TestBase
    {
        [Test]
        public void Can_delete_document_on_server()
        {
            Application.Execute((browser, context) =>
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
        public void Can_read_document_on_server()
        {
            Application.Execute((browser, context) =>
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
        public void Can_create_document_on_server()
        {
            Application.Execute((browser, context) =>
            {
                // Act.
                var response = browser.Post(CrowbarRoute.Root, ctx => ctx.FormValue("text", "New Crowbar"));

                // Assert.
                using (var session = context.Store.OpenSession())
                {
                    dynamic json = response.ShouldBeJson();
                    var model = session.Load<Model>((string)json.id);

                    Assert.That(model, Is.Not.Null);
                    Assert.That(model.Text, Is.EqualTo("New Crowbar"));
                }
            });
        }

        [Test]
        public void Should_get_a_new_instance_of_document_store_for_each_run_1()
        {
            Application.Execute((browser, context) =>
            {
                // Arrange.
                const string text = "crowbar";

                var model = new Model { Text = text };
                using (var raven = context.Store.OpenSession())
                {
                    raven.Store(model);
                    raven.SaveChanges();
                }

                // Act.
                var response = browser.Get(CrowbarRoute.DocumentStore, ctx => ctx.Query("text", text));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public void Should_get_a_new_instance_of_document_store_for_each_run_2()
        {
            Application.Execute((browser, context) =>
            {
                // Arrange.
                const string text = "crowbar";

                var model = new Model { Text = text };
                using (var raven = context.Store.OpenSession())
                {
                    raven.Store(model);
                    raven.SaveChanges();
                }

                // Act.
                var response = browser.Get(CrowbarRoute.DocumentStore, ctx => ctx.Query("text", text));

                // Assert.
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}