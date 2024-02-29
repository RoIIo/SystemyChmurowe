using Chmura.Database.Entities;
using Chmura.Dto;
using Chmura.ORM;
using Chmura.Repository;
using CsvHelper;
using System.Globalization;

namespace Chmura.Domain
{
    public interface ICSVReader
	{
		void LoadData(string? dataPath);
	}
	public class CSVReader : ICSVReader
	{
		private readonly IHoneyRepository honeyRepository;
		private readonly IPollenRepository pollenRepository;
		private readonly ITransactionCoordinator transactionCoordinator;
		public CSVReader(ITransactionCoordinator transactionCoordinator, IHoneyRepository honeyRepository, IPollenRepository pollenRepository)
		{
			this.transactionCoordinator = transactionCoordinator;
			this.honeyRepository = honeyRepository;
			this.honeyRepository = honeyRepository;
			this.pollenRepository = pollenRepository;
		}
		public void LoadData(string? dataPath)
		{
			if (dataPath == null) return;
			transactionCoordinator.InCommitScope(session =>
			{
				var pollenCache =new Dictionary<string, Pollen>();
				var csv = new CsvReader(new StreamReader(dataPath), CultureInfo.InvariantCulture);
				var records = csv.GetRecords<HoneyRow>();
				foreach (var record in records)
				{
					Pollen? pollen = null;
					if(record.Pollen_analysis != null && !pollenCache.TryGetValue(record.Pollen_analysis, out pollen))
					{
						pollen = new Pollen() { Name = record?.Pollen_analysis, HoneyList = new List<Honey>() };
						pollenRepository.Insert(pollen, session).GetAwaiter().GetResult();
						pollenCache.Add(record.Pollen_analysis, pollen);
					}
				
					var honey = new Honey()
					{
						CS = record.CS,
						Density = record.Density,
						WC = record.WC,
						pH = record.pH,
						EC = record.EC,
						F = record.F,
						G = record.G,
						PollenAnalysis = pollen,
						Viscosity = record.Viscosity,
						Purity = record.Purity
					};
					pollen?.HoneyList?.Add(honey);
					honeyRepository.Insert(honey, session).GetAwaiter().GetResult();
				}
				foreach(var pollen in pollenCache.Values)
				{
					pollenRepository.Update(pollen, session).GetAwaiter().GetResult();
				}
			});
		}
        private class HoneyRow
        {
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
		}
    }
}
