using Chmura.Database.Entities;
using NHibernate.Linq;
using ISession = NHibernate.ISession;


namespace Chmura.Repository
{
	public interface IHoneyRepository
	{
		Task<Honey> GetById(int id, ISession session);
		Task<IList<Honey>> GetAll(ISession session);
		Task Delete(Honey honey, ISession session);
		Task DeleteById(int id, ISession session);
		Task Insert(Honey honey, ISession session);
		Task Update(Honey honey, ISession session);
	}
	public class HoneyRepository : IHoneyRepository
	{
		public async Task<Honey> GetById(int id, ISession session)
		{
			return	await session.GetAsync<Honey>(id);
		}
		public async Task<IList<Honey>> GetAll(ISession session)
		{
			return await session.Query<Honey>().ToListAsync();
		}
		public async Task Delete(Honey honey, ISession session)
		{
			await session.DeleteAsync(honey);
		}
		public async Task DeleteById(int id, ISession session)
		{
			var honey = await GetById(id, session);
			if (honey != null)
			{
				await session.DeleteAsync(honey);
			}
		}
		public async Task Insert(Honey honey, ISession session)
		{
			await session.SaveOrUpdateAsync(honey);
		}
		public async Task Update(Honey honey, ISession session)
		{
			await session.UpdateAsync(honey);
		}

	}
}
