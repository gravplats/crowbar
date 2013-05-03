namespace Crowbar.Demo.Mvc.WebApi.Tests
{
    public class As_a_user_requesting_crowbars : UserStory
    {
        [Then("should receive a list of crowbars")]
        protected override void Test()
        {
            Application.Execute(client =>
            {
                var response = client.Get("api/crowbars");
                response.ShouldHaveStatusCode(HttpStatusCode.OK);
            });
        }
    }
}
