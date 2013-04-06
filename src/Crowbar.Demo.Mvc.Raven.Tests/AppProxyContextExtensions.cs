using Crowbar.Demo.Mvc.Raven.Application.Models;

namespace Crowbar.Demo.Mvc.Raven.Tests
{
    public static class AppProxyContextExtensions
    {
        public static User User(this AppProxyContext context, string username, string password)
        {
            using (var session = context.Store.OpenSession())
            {
                var user = new User(username, password);

                session.Store(user);
                session.SaveChanges();

                return user;
            }
        }
    }
}