using NUnit.Framework;

namespace Crowbar.Demo.WebApi.Tests
{
    public class As_a_user_requesting_crowbars : UserStory
    {
        [Then("should receive a list of crowbars")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                var response = client.Get("api/crowbars");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
