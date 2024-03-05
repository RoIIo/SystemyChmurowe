using Chmura.Database.Entities;
using Chmura.Domain;
using NHibernate.Linq;
using ISession = NHibernate.ISession;


namespace Chmura.Repository
{
	public interface IHoneyRepository
	{
		Task<Honey> GetById(int id, ISession session);
		Task<IList<Honey>> GetPage(int page, int pageSize, HoneyFilter filter, ISession session);
		Task<int> GetTotalEntities(HoneyFilter filter, ISession session);
		Task Delete(Honey honey, ISession session);
		Task DeleteById(int id, ISession session);
		Task Insert(Honey honey, ISession session);
		Task Update(Honey honey, ISession session);
	}
	public class HoneyRepository : IHoneyRepository
	{
		public async Task<int> GetTotalEntities(HoneyFilter filter, ISession session)
		{
			return await session.Query<Honey>().AddWhere(filter).CountAsync();
		}
		public async Task<Honey> GetById(int id, ISession session)
		{
			return	await session.GetAsync<Honey>(id);
		}
		public async Task<IList<Honey>> GetPage(int page, int pageSize, HoneyFilter filter, ISession session)
		{
			return await session.Query<Honey>().AddWhere(filter).Skip(page * pageSize).Take(pageSize).ToListAsync();
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

	public static class QueryExtension
	{
		public static IQueryable<Honey> AddWhere(this IQueryable<Honey> query, HoneyFilter filter)
		{
			if (filter.MinId != null)
			{
				query = query.Where(x => x.Id >= filter.MinId);
			}
			if (filter.MaxId != null)
			{
				query = query.Where(x => x.Id <= filter.MaxId);
			}
			if (filter.MinCS != null)
			{
				query = query.Where(x => x.CS >= filter.MinCS);
			}
			if (filter.MaxCS != null)
			{
				query = query.Where(x => x.CS <= filter.MaxCS);
			}
			if (filter.MinDensity != null)
			{
				query = query.Where(x => x.Density >= filter.MinDensity);
			}
			if (filter.MaxDensity != null)
			{
				query = query.Where(x => x.Density <= filter.MaxDensity);
			}
			if (filter.MinWC != null)
			{
				query = query.Where(x => x.WC >= filter.MinWC);
			}
			if (filter.MaxWC != null)
			{
				query = query.Where(x => x.WC <= filter.MaxWC);
			}
			if (filter.MinpH != null)
			{
				query = query.Where(x => x.pH >= filter.MinpH);
			}
			if (filter.MaxpH != null)
			{
				query = query.Where(x => x.pH <= filter.MaxpH);
			}
			if (filter.MinEC != null)
			{
				query = query.Where(x => x.EC >= filter.MinEC);
			}
			if (filter.MaxEC != null)
			{
				query = query.Where(x => x.EC <= filter.MaxEC);
			}
			if (filter.MinF != null)
			{
				query = query.Where(x => x.F >= filter.MinF);
			}
			if (filter.MaxF != null)
			{
				query = query.Where(x => x.F <= filter.MaxF);
			}
			if (filter.MinG != null)
			{
				query = query.Where(x => x.G >= filter.MinG);
			}
			if (filter.MaxG != null)
			{
				query = query.Where(x => x.G <= filter.MaxG);
			}
			if (filter.MinViscosity != null)
			{
				query = query.Where(x => x.Viscosity >= filter.MinViscosity);
			}
			if (filter.MaxViscosity != null)
			{
				query = query.Where(x => x.Viscosity <= filter.MaxViscosity);
			}
			if (filter.MinPurity != null)
			{
				query = query.Where(x => x.Purity >= filter.MinPurity);
			}
			if (filter.MaxPurity != null)
			{
				query = query.Where(x => x.Purity <= filter.MaxPurity);
			}
			if (filter.PollenAnalysis != null)
			{
				query = query.Where(x => x.PollenAnalysis.Name == filter.PollenAnalysis);
			}
			return query;
		}
	}

}
