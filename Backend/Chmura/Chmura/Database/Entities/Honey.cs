using Chmura.Dto;

namespace Chmura.Database.Entities
{
	public class Honey : IToDto<HoneyDto>
	{
		public virtual int Id { get; set; }
		public virtual float? CS { get; set; }
		public virtual float? Density { get; set; }
		public virtual float? WC { get; set; }
		public virtual float? pH { get; set; }
		public virtual float? EC { get; set; }
		public virtual float? F { get; set; }
		public virtual float? G { get; set; }
		public virtual Pollen? PollenAnalysis { get; set; }
		public virtual float? Viscosity { get; set; }
		public virtual float? Purity { get; set; }

		public virtual HoneyDto ToDto()
		{
			var dto = new HoneyDto()
			{
				Id = Id,
				CS = CS,
				Density = Density,
				WC = WC,
				pH = pH,
				EC = EC,
				F = F,
				G = G,
				Pollen_analysis = PollenAnalysis?.Name,
				Viscosity = Viscosity,
				Purity = Purity
			};
			return dto;
		}
	}
}
