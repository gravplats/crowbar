using Raven.Web;

namespace Raven.Tests
{
    public static class RavenProxyContextExtensions
    {
        public static User User(this RavenProxyContext context, string username, string password)
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