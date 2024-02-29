using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using ISession = NHibernate.ISession;

namespace Chmura.ORM
{
    public interface INHibernateHelper
    {
        ISession OpenSession();
        void CloseSession(ISession session);
        void CloseSessionFactory();
    }
    public class NHibernateHelper : INHibernateHelper
    {
        private ISessionFactory sessionFactory;
        public NHibernateHelper()
        {
            sessionFactory = FluentConfigure();
        }

        public ISession OpenSession()
        {
            return sessionFactory.OpenSession();
        }
        public void CloseSession(ISession session)
        {
            session.Close();
        }
        public void CloseSessionFactory()
        {
            sessionFactory.Close();
        }
        private ISessionFactory FluentConfigure()
        {
			var configuration = Fluently.Configure()
				.Database(GetDatabaseConfiguration)
				.Mappings(m =>
					m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
				.Cache(
					c => c.UseQueryCache()
					.UseSecondLevelCache()
					.ProviderClass<NHibernate.Cache.HashtableCacheProvider>())
				.BuildConfiguration();
			var exporter = new SchemaExport(configuration);
			exporter.Execute(true, true, false);

			return configuration.BuildSessionFactory();
		}
		private IPersistenceConfigurer GetDatabaseConfiguration()
        {
            return PostgreSQLConfiguration.PostgreSQL83.ConnectionString(c =>
            {
                c.Host("127.0.0.1");
                c.Database("CHMURA");
                c.Port(5432);
                c.Username("CHMURAUser");
                c.Password("CHMURAPassword");
            }).AdoNetBatchSize(5000);
        }
    }
}
