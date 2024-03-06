using Chmura.Database.Entities;
using Chmura.Dto;
using Chmura.ORM;
using Chmura.Repository;
using CsvHelper;
using System.Globalization;
using System.Reflection;
using ISession = NHibernate.ISession;


namespace Chmura.Domain
{
    public interface ICSVReader
	{
		Task LoadDataAsync();
	}
	public class CSVReader : ICSVReader
	{
		private readonly IHoneyRepository honeyRepository;
		private readonly IPollenRepository pollenRepository;
		private readonly ITransactionCoordinator transactionCoordinator;
		private readonly string? dataPath;
		public CSVReader(ITransactionCoordinator transactionCoordinator, IHoneyRepository honeyRepository, IPollenRepository pollenRepository, IConfiguration configuration)
		{
			this.transactionCoordinator = transactionCoordinator;
			this.honeyRepository = honeyRepository;
			this.honeyRepository = honeyRepository;
			this.pollenRepository = pollenRepository;
			dataPath = configuration.GetSection("DataPath").Value?.ToString();
		}
		public async Task LoadDataAsync()
		{
			if (dataPath == null) return;
			var finalDataPath = dataPath;
			if (!Path.IsPathRooted(dataPath))
			{
				finalDataPath = Path.Combine(Directory.GetCurrentDirectory(), dataPath);
			}

			await transactionCoordinator.InCommitScopeAsync(async session =>
			{
				await DeleteOldDataAsync(session);
				await InsertNewDataAsync(finalDataPath, session);
			});
		}

		private async Task InsertNewDataAsync(string finalDataPath, ISession session)
		{
			var pollenCache = new Dictionary<string, Pollen>();
			var csv = new CsvReader(new StreamReader(finalDataPath), CultureInfo.InvariantCulture);

			await foreach (var record in csv.GetRecordsAsync<HoneyRow>())
			{
				Pollen? pollen = null;
				if (record.Pollen_analysis != null && !pollenCache.TryGetValue(record.Pollen_analysis, out pollen))
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
				await honeyRepository.Insert(honey, session);
			}
			foreach (var pollen in pollenCache.Values)
			{
				await pollenRepository.Update(pollen, session);
			}
		}

		private async Task DeleteOldDataAsync(ISession session)
		{
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.Namespace == "Chmura.Database.Entities").ToList();
			foreach (Type myObject in types)
			{
				await session.CreateQuery($"Delete from {myObject.Name}").ExecuteUpdateAsync();
			}
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
