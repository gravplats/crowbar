using System.IO;
using Crowbar.Demo.Mvc.NHibernate.Application;
using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate;
using Crowbar.Demo.Mvc.NHibernate.Tests.Infrastructure.Modules;
using Crowbar.Demo.Mvc.NHibernate.Tests.Infrastructure.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Ninject;

namespace Crowbar.Demo.Mvc.NHibernate.Tests
{
    public class AppProxy : MvcApplicationProxyBase<App, AppProxyContext>
    {
        protected override void OnApplicationStart(App application, string testBaseDirectory)
        {
            application.Kernel.Load(new AppProxyNHibernateModule());
        }

        protected override AppProxyContext CreateContext(App application, string testBaseDirectory)
        {
            var kernel = application.Kernel;
            var session = GetNHibernateSession(kernel);

            return new AppProxyContext(session);
        }

        private static ISession GetNHibernateSession(IKernel kernel)
        {
            var configuration = kernel.Get<Configuration>();
            var export = new SchemaExport(configuration);

            var session = kernel.Get<ISessionFactory>().OpenSession();
            session.BeginTransaction();

            export.Execute(script: false, export: true, justDrop: false, connection: session.Connection, exportOutput: TextWriter.Null);

            var request = new AppProxyNHibernateSessionPerRequest(session);
            kernel.Rebind<NHibernateSessionPerRequest>().ToConstant(request);

            return session;
        }
    }
}