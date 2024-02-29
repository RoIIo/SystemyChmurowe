using Chmura.Database.Entities;
using ISession = NHibernate.ISession;

namespace Chmura.Repository
{
	public interface IPollenRepository
	{
		Task Insert(Pollen pollen, ISession session);
		Task<Pollen> GetPollenByName(string name, ISession session);
		Task Update(Pollen pollen, ISession session);

	}
	public class PollenRepository : IPollenRepository
	{
		public async Task Insert(Pollen pollen, ISession session)
		{
			await session.SaveOrUpdateAsync(pollen);
		}
		public async Task<Pollen> GetPollenByName(string name, ISession session)
		{
			return await session.QueryOver<Pollen>().Where(p => p.Name == name).SingleOrDefaultAsync();
		}
		public async Task Update(Pollen pollen, ISession session)
		{
			await session.UpdateAsync(pollen);
		}
	}
}
