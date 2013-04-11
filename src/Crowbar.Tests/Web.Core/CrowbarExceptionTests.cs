using System;
using Crowbar.Tests.Mvc.Common;
using NUnit.Framework;

namespace Crowbar.Tests.Web.Core
{
    public class CrowbarExceptionTests : TestBase
    {
        [Test]
        public void Should_be_able_to_handle_non_serializable_exceptions()
        {
            Application.Execute(client =>
            {
                Assert.Throws<Exception>(() => client.Post(CrowbarRoute.ExceptionNonSerializable));
            });
        }

        [Test]
        public void Should_be_able_to_handle_serializable_exceptions()
        {
            Application.Execute(client =>
            {
                Assert.Throws<CrowbarException>(() => client.Post(CrowbarRoute.ExceptionSerializable));
            });
        }
    }
}