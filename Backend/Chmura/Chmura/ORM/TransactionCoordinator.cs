using Chmura.ORM;
using ISession = NHibernate.ISession;

namespace Chmura.ORM
{
    public interface ITransactionCoordinator
	{
		Task<T> InRollbackScopeAsync<T>(Func<ISession, Task<T>> action);
		Task InRollbackScopeAsync(Func<ISession, Task> action);
		T InRollbackScope<T>(Func<ISession, T> action);
		void InRollbackScope(Action<ISession> action);
		void InCommitScope(Action<ISession> action);
		T InCommitScope<T>(Func<ISession, T> action);
		Task<T> InCommitScopeAsync<T>(Func<ISession, Task<T>> action);
		Task InCommitScopeAsync(Func<ISession, Task> action);
	}

	public class TransactionCoordinator : ITransactionCoordinator
	{
		private INHibernateHelper nHibernateHelper;
		private ILogger<TransactionCoordinator> logger;

		public TransactionCoordinator(
			INHibernateHelper nHibernateHelper,
			ILogger<TransactionCoordinator> logger)
		{
			this.nHibernateHelper = nHibernateHelper;
			this.logger = logger;
		}


		public async Task<T> InRollbackScopeAsync<T>(Func<ISession, Task<T>> action)
		{
			T? result;
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					result = await action(session);
				}
				catch
				{
					throw;
				}
				finally
				{
					transaction?.Rollback();
					session?.Close();
				}
				return result;
			}
		}
		public async Task InRollbackScopeAsync(Func<ISession, Task> action)
		{
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					await action(session);
				}
				catch
				{
					throw;
				}
				finally
				{
					await transaction?.RollbackAsync();
					session?.Close();
				}
			}
		}

		public T InRollbackScope<T>(Func<ISession, T> action)
		{
			T? result;
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					result = action(session);
				}
				catch
				{
					throw;
				}
				finally
				{
					transaction?.Rollback();
					session?.Close();
				}
				return result;
			}
		}

		public void InRollbackScope(Action<ISession> action)
		{
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					action(session);
				}
				catch
				{
					throw;
				}
				finally
				{
					transaction?.Rollback();
					session?.Close();
				}
			}
		}
		public void InCommitScope(Action<ISession> action)
		{
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					action(session);
					transaction.Commit();
				}
				catch
				{
					transaction?.Rollback();
					throw;
				}
				finally
				{
					session?.Close();
				}
			}
		}
		public T InCommitScope<T>(Func<ISession, T> action)
		{
			T? result;
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					result = action(session);
					transaction.Commit();
				}
				catch
				{
					transaction?.Rollback();
					throw;
				}
				finally
				{
					session?.Close();
				}
			}
			return result;
		}

		public async Task<T> InCommitScopeAsync<T>(Func<ISession, Task<T>> action)
		{
			T? result;
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					result = await action(session);
					await transaction.CommitAsync();
				}
				catch
				{
					await transaction?.RollbackAsync();
					throw;
				}
				finally
				{
					session?.Close();
				}

				return result;

			}
		}
		public async Task InCommitScopeAsync(Func<ISession, Task> action)
		{
			using (var session = nHibernateHelper.OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				try
				{
					await action(session);
					await transaction.CommitAsync();
				}
				catch
				{
					await transaction?.RollbackAsync();
					throw;
				}
				finally
				{
					session?.Close();
				}
			}
		}
	}
}
