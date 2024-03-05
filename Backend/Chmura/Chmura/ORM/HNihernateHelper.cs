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
        private string connectionString;
        public NHibernateHelper(IConfiguration configuration)
        {
			connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
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
            return PostgreSQLConfiguration.PostgreSQL83.ConnectionString(connectionString).AdoNetBatchSize(5000);
        }
    }
}
