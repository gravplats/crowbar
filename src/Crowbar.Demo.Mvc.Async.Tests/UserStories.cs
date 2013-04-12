using Crowbar.Demo.Mvc.Async.Application;
using NUnit.Framework;

namespace Crowbar.Demo.Mvc.Async.Tests
{
    public class As_a_user_requesting_an_async_endpoint : UserStory
    {
        [Then("should receive HTTP Status 200 OK")]
        protected override void Test()
        {
            Application.Execute((client, context) =>
            {
                var response = client.Get(AppRoute.External);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}