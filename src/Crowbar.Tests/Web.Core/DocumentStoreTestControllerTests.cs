using Crowbar.Web.Core;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class DocumentStoreTestControllerTests : TestBase
    {
        [Test]
        public void Should_get_a_new_instance_of_document_store_for_each_run_1()
        {
            Application.Execute((context, browser) =>
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
            Application.Execute((context, browser) =>
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