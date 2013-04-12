using System;
using Crowbar.Demo.Mvc.Application;
using NUnit.Framework;

namespace Crowbar.Demo.Mvc.Tests
{
    [TestFixture]
    public abstract class UserStory
    {
        [Serializable]
        public class AppProxyHttpPayloadDefaults : IHttpPayloadDefaults
        {
            public void ApplyTo(HttpPayload payload)
            {
                payload.HttpsRequest();
            }
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public class ThenAttribute : Attribute
        {
            public ThenAttribute(string description)
            {
                Description = description;
            }

            public string Description { get; private set; }
        }

        protected static readonly MvcApplication<App> Application =
            MvcApplication.Create<App>("Crowbar.Demo.Mvc", "Web.Custom.config", new AppProxyHttpPayloadDefaults());

        [Test]
        public void Execute()
        {
            Test();
        }

        protected abstract void Test();
    }
}