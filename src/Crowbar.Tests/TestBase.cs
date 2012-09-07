namespace Crowbar.Tests
{
    public abstract class TestBase
    {
        // Creating the MVC-application is a time-consuming process and should preferably only be done once.
        protected static readonly MvcApplication<RavenContext> Application =
            MvcApplication.Create<RavenMvcApplicationProxy, RavenContext>("Crowbar.Web", "Web.Custom.config");
    }
}