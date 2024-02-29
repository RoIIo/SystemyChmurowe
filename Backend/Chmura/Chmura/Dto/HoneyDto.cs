using Chmura.Database.Entities;

namespace Chmura.Dto
{
    public class HoneyDto : IToEntity<Honey>
    {
        public int? Id { get; set; }
        public float? CS { get; set; }
        public float? Density { get; set; }
        public float? WC { get; set; }
        public float? pH { get; set; }
        public float? EC { get; set; }
        public float? F { get; set; }
        public float? G { get; set; }
        public virtual string? Pollen_analysis { get; set; }
        public float? Viscosity { get; set; }
        public float? Purity { get; set; }

		public Honey ToEntity()
		{
			var honey = new Honey()
            {
				Id = Id ?? 0,
				CS = CS,
				Density = Density,
				WC = WC,
				pH = pH,
				EC = EC,
				F = F,
				G = G,
				Viscosity = Viscosity,
				Purity = Purity
			};
			return honey;
		}
	}
}
