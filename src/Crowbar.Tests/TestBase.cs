using System;

namespace Crowbar.Tests
{
    public abstract class TestBase
    {
        // Creating the MVC-application is a time-consuming process and should preferably only be done once.
        protected static readonly MvcApplication[] Applications = new[]
        {
            MvcApplication.Create("Crowbar.Tests.Mvc3", "Web.Custom.config"),
            MvcApplication.Create("Crowbar.Tests.Mvc4", "Web.Custom.config"),
            MvcApplication.Create("Crowbar.Tests.Mvc5", "Web.Custom.config")
        };

        protected void Execute(Action<Client> client)
        {
            foreach (var application in Applications)
            {
                application.Execute(client);
            }
        }
    }
}