using System;
using Crowbar.Demo.Mvc.Async.Application;
using NUnit.Framework;

namespace Crowbar.Demo.Mvc.Async.Tests
{
    [TestFixture]
    public abstract class UserStory
    {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public class ThenAttribute : Attribute
        {
            public ThenAttribute(string description)
            {
                Description = description;
            }

            public string Description { get; private set; }
        }

        protected static readonly MvcApplication<App, AppProxyContext> Application =
            MvcApplication.Create<App, AppProxy, AppProxyContext>("Crowbar.Demo.Mvc.Async", "Web.Custom.config", payload => payload.HttpsRequest());

        [Test]
        public void Execute()
        {
            Test();
        }

        protected abstract void Test();
    }
}