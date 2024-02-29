using Chmura.Database.Entities;
using FluentNHibernate.Mapping;

namespace Chmura.Database.Mapping
{
	public class PollenMap : ClassMap<Pollen>
	{
		public PollenMap()
		{
			Id(x => x.Id).Not.Nullable().GeneratedBy.Increment();
			Map(x => x.Name);
			HasMany(x => x.HoneyList)
				.Table("Honey")
				.Inverse()
				.Cascade.AllDeleteOrphan()
				.Not.LazyLoad();

			Table("Pollen");
		}
	}
}
