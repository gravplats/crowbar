using System;
using System.Web;
using System.Web.Mvc;

namespace Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate
{
    public class NHibernateSessionPerRequestAdapter
    {
        class Request<THttpApplication>
            where THttpApplication : HttpApplication, INHibernateSessionKey
        {
            private readonly THttpApplication application;

            internal Request(THttpApplication application)
            {
                this.application = application;
            }

            public void OnBeginRequest(object sender, EventArgs e)
            {
                var request = DependencyResolver.Current.GetService<NHibernateSessionPerRequest>();
                request.OnBeginRequest(application);
            }

            public void OnEndRequest(object sender, EventArgs e)
            {
                var request = DependencyResolver.Current.GetService<NHibernateSessionPerRequest>();
                request.OnEndRequest(application);
            }
        }

        public static void Register<THttpApplication>(THttpApplication application)
            where THttpApplication : HttpApplication, INHibernateSessionKey
        {
            var request = new Request<THttpApplication>(application);
            application.BeginRequest += request.OnBeginRequest;
            application.EndRequest += request.OnEndRequest;
        }
    }
}