using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Crowbar.Demo.Mvc.NHibernate.Application.Models
{
    public class UserMapping : ClassMapping<User>
    {
        public UserMapping()
        {
            Table("Users");

            Id(x => x.Id, m => { m.Generator(new NativeGeneratorDef()); });

            Property(x => x.Username, m =>
            {
                m.Column(c =>
                {
                    c.Length(256);
                    c.NotNullable(true);
                    c.SqlType("varchar(256)");
                });
            });

            Component(x => x.Password, m =>
            {
                m.Property(x => x.Hash, pm =>
                {
                    pm.Column(c =>
                    {
                        c.Length(256);
                        c.NotNullable(true);
                        c.SqlType("varchar(256)");
                    });
                });
            });
        }
    }
}