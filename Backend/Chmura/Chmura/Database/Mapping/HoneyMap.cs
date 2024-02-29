using Chmura.Database.Entities;
using FluentNHibernate.Mapping;

namespace Chmura.Database.Mapping
{
	public class HoneyMap : ClassMap<Honey>
	{
		public HoneyMap()
		{
			Id(x => x.Id).Not.Nullable().GeneratedBy.Increment();
			Map(x => x.CS);
			Map(x => x.Density);
			Map(x => x.WC);
			Map(x => x.pH);
			Map(x => x.EC);
			Map(x => x.F);
			Map(x => x.G);
			Map(x => x.Viscosity);
			Map(x => x.Purity);

			References(x => x.PollenAnalysis)
				.Column("PollenAnalysis_id")
				.Not.LazyLoad();
			Table("Honey");
		}
	}
}
