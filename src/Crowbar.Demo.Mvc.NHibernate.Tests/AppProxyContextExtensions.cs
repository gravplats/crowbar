using Crowbar.Demo.Mvc.NHibernate.Application.Models;

namespace Crowbar.Demo.Mvc.NHibernate.Tests
{
    public static class AppProxyContextExtensions
    {
        public static User User(this AppProxyContext context, string username, string password)
        {
            var user = new User(username, password);
            context.Session.Save(user);

            return user;
        }
    }
}