using Chmura.Database.Entities;

namespace Chmura.Domain
{
	public class HoneyFilter
	{
		public int? MinId { get; set; }
		public int? MaxId { get; set; }
		public float? MinCS { get; set; }
		public float? MaxCS { get; set; }
		public float? MinDensity { get; set; }
		public float? MaxDensity { get; set; }
		public float? MinWC { get; set; }
		public float? MaxWC { get; set; }
		public float? MinpH { get; set; }
		public float? MaxpH { get; set; }
		public float? MinEC { get; set; }
		public float? MaxEC { get; set; }
		public float? MinF { get; set; }
		public float? MaxF { get; set; }
		public float? MinG { get; set; }
		public float? MaxG { get; set; }
		public string? PollenAnalysis { get; set; }
		public float? MinViscosity { get; set; }
		public float? MaxViscosity { get; set; }
		public float? MinPurity { get; set; }
		public float? MaxPurity { get; set; }
	}
}
