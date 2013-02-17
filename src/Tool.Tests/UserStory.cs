using System;
using Crowbar;
using NUnit.Framework;

namespace Tool.Tests
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

        protected static readonly MvcApplication Application = MvcApplication.Create("Tool.Web", "Web.Custom.config", ctx => ctx.HttpsRequest());

        [Test]
        public void Execute()
        {
            Test();
        }

        protected abstract void Test();
    }
}