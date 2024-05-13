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
		private readonly ILogger<NHibernateHelper> logger;
        private ISessionFactory sessionFactory;
        private string connectionString;
        public NHibernateHelper(IConfiguration configuration, ILogger<NHibernateHelper> logger)
        {
            logger = logger;
			connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            logger.LogInformation("Connection string: {0}", connectionString);
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
				.ExposeConfiguration(cfg =>
				{
					new SchemaUpdate(cfg).Execute(false, true);
				})
				.BuildConfiguration();

			return configuration.BuildSessionFactory();
		}
		private IPersistenceConfigurer GetDatabaseConfiguration()
        {
            return PostgreSQLConfiguration.PostgreSQL83.ConnectionString(connectionString).AdoNetBatchSize(5000);
        }
    }
}
