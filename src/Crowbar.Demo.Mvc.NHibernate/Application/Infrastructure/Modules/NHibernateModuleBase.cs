using Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.NHibernate;
using Crowbar.Demo.Mvc.NHibernate.Application.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using Ninject.Modules;

namespace Crowbar.Demo.Mvc.NHibernate.Application.Infrastructure.Modules
{
    public class NHibernateModuleBase : NinjectModule
    {
        public override void Load()
        {
            Bind<NHibernateSessionPerRequest>().ToSelf().InSingletonScope();

            var configuration = Configure();
            Bind<Configuration>().ToConstant(configuration).InSingletonScope();

            var factory = configuration.BuildSessionFactory();
            Bind<ISessionFactory>().ToConstant(factory).InSingletonScope();
        }

        protected virtual Configuration Configure()
        {
            var config = new Configuration();
            config.Properties.Clear();

            Customize(config);

            var mapper = new ModelMapper();
            mapper.AddMapping(typeof(UserMapping));

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            config.AddMapping(mapping);

            return config;
        }

        protected virtual void Customize(Configuration config) { }
    }
}