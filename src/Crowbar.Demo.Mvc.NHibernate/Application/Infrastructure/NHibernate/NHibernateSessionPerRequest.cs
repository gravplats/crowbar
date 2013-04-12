using System.Web;
using System.Web.Mvc;
using NHibernate;

namespace Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate
{
    public class NHibernateSessionPerRequest
    {
        public void OnBeginRequest<THttpApplication>(THttpApplication application)
            where THttpApplication : HttpApplication, INHibernateSessionKey
        {
            string key = application.GetNHibernateSessionKey();
            HttpContext.Current.Items[key] = GetSessionOnBeginRequest();
        }

        public void OnEndRequest<THttpApplication>(THttpApplication application)
            where THttpApplication : HttpApplication, INHibernateSessionKey
        {
            var session = GetSessionOnEndRequest(application);

            try
            {
                if (application.Server.GetLastError() != null)
                {
                    session.Transaction.Rollback();
                }
                else
                {
                    session.Transaction.Commit();
                }
            }
            finally
            {
                DisposeSessionOnEndRequest(session);
            }
        }

        protected virtual ISession GetSessionOnBeginRequest()
        {
            return DependencyResolver.Current.GetService<ISessionFactory>().OpenSession();
        }

        protected virtual ISession GetSessionOnEndRequest<THttpApplication>(THttpApplication application)
            where THttpApplication : HttpApplication, INHibernateSessionKey
        {
            string key = application.GetNHibernateSessionKey();
            return (ISession)HttpContext.Current.Items[key];
        }

        protected virtual void DisposeSessionOnEndRequest(ISession session)
        {
            session.Dispose();
        }
    }
}