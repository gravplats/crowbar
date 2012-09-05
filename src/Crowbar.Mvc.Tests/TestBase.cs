namespace Crowbar.Mvc.Tests
{
    public abstract class TestBase
    {
        // Creating the server is a time-consuming process and should preferably only be done once.
        protected static readonly Server Server = Server.Create("Crowbar.Mvc", "Web.Custom.config");
    }
}