using System;
using NHibernate;

namespace Crowbar.Demo.Mvc.NHibernate.Tests
{
    public class AppProxyContext : IDisposable
    {
        public AppProxyContext(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}