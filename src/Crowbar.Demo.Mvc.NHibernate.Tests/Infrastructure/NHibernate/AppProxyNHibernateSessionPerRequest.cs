using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate;
using NHibernate;

namespace Crowbar.Demo.Mvc.NHibernate.Tests.Infrastructure.NHibernate
{
    public class AppProxyNHibernateSessionPerRequest : NHibernateSessionPerRequest
    {
        private readonly ISession session;

        public AppProxyNHibernateSessionPerRequest(ISession session)
        {
            this.session = session;
        }

        protected override ISession GetSessionOnBeginRequest()
        {
            return session;
        }

        protected override ISession GetSessionOnEndRequest<THttpApplication>(THttpApplication application)
        {
            return session;
        }

        protected override void DisposeSessionOnEndRequest(ISession _)
        {
            // Don't dispose the session here in user stories. Let the AppProxyContext dispose it instead.
        }
    }
}