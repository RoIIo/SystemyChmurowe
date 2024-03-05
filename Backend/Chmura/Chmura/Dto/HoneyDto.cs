using Chmura.Database.Entities;

namespace Chmura.Dto
{
    public class HoneyDto
    {
        public int? Id { get; set; }
        public float? CS { get; set; }
        public float? Density { get; set; }
        public float? WC { get; set; }
        public float? pH { get; set; }
        public float? EC { get; set; }
        public float? F { get; set; }
        public float? G { get; set; }
        public string? Pollen_analysis { get; set; }
        public float? Viscosity { get; set; }
        public float? Purity { get; set; }
	}
}
