using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.Modules;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Crowbar.Demo.Mvc.NHibernate.Tests.Infrastructure.Modules
{
    public class AppProxyNHibernateModule : NHibernateModuleBase
    {
        private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;";

        protected override void Customize(Configuration config)
        {
            config.DataBaseIntegration(x =>
            {
                x.ConnectionString = ConnectionString;
                x.Dialect<SQLiteDialect>();
                x.Driver<SQLite20Driver>();
            });

            config.SetProperty("cache.use_second_level_cache", "false");
            config.SetProperty("generate_statistics", "true");
            config.SetProperty("auto-import", "false");
        }         
    }
}