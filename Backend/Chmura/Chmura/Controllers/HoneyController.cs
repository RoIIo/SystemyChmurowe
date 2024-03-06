using Chmura.Database.Entities;
using Chmura.Database.Mapping;
using Chmura.Domain;
using Chmura.Dto;
using Chmura.ORM;
using Chmura.Repository;
using FluentNHibernate.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;
using ISession = NHibernate.ISession;


namespace Chmura.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HoneyController : ControllerBase
	{
		private readonly ILogger<HoneyController> _logger;
		private readonly ITransactionCoordinator transactionCoordinator;
		private readonly IHoneyRepository honeyRepository;
		private readonly IPollenRepository pollenRepository;
		private readonly ICSVReader reader;

		public HoneyController(ILogger<HoneyController> logger, ITransactionCoordinator transactionCoordinator, IHoneyRepository honeyRepository, IPollenRepository pollenRepository, ICSVReader reader)
		{
			this.transactionCoordinator = transactionCoordinator;
			this.honeyRepository = honeyRepository;
			_logger = logger;
			this.pollenRepository = pollenRepository;
			this.reader = reader;
		}

		[HttpPost("ReloadData", Name = "ReloadData")]
		public async Task<ActionResult<string>> ReloadData()
		{
			await reader.LoadDataAsync();
			return Ok("Data reloaded");
		}

		[HttpGet("GetTotalEntities", Name = "GetTotalEntities")]
		public async Task<ActionResult<int>> GetTotalEntities([FromQuery] HoneyFilter filter)
		{
			int result = 0;
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				result = await honeyRepository.GetTotalEntities(filter, session);
			});
			return Ok(result);
		}

		[HttpGet("GetStatistics", Name = "GetStatistics")]
		public async Task<ActionResult<Statistics>> GetStatistics([FromQuery] HoneyFilter filter)
		{
			Statistics statistics = new Statistics();
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				var entities = await honeyRepository.GetFilteredEntities(filter, session);
				statistics = CalculateStatistics(entities);
			});
			return Ok(statistics);
		}

		[HttpGet("GetAll", Name = "GetAll")]
		public async Task<ActionResult<IList<HoneyDto>>> GetAll([FromQuery] int page = 0, [FromQuery] int pageSize = 100, [FromQuery] HoneyFilter filter = null)
		{
			IList<HoneyDto> result = new List<HoneyDto>();
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				var honeyList = await honeyRepository.GetPage(page, pageSize, filter, session);
				result = honeyList.Select(honey => honey.ToDto()).ToList();
			});
			return Ok(result);
		}

		[HttpPost("AddHoney", Name = "AddHoney")]
		public async Task<ActionResult<HoneyDto>> AddHoney(HoneyDto honeyDto)
		{
			await transactionCoordinator.InCommitScopeAsync(async session =>
			{
				var honey = await ToEntity(honeyDto);
				await honeyRepository.Insert(honey, session);
			});
			return Ok("Honey was added");
		}

		[HttpPut("UpdateHoney", Name = "UpdateHoney")]
		public async Task<ActionResult<HoneyDto>> UpdateHoney(HoneyDto honeyDto)
		{
			var existingHoney = await transactionCoordinator.InRollbackScopeAsync(async session => await honeyRepository.GetById(honeyDto.Id ?? 0, session));
			if (existingHoney == null)
			{
				return NotFound("Honey not found");
			}

			await transactionCoordinator.InCommitScopeAsync(async session =>
			{
				await UpdateHoney(honeyDto, existingHoney);
				await honeyRepository.Update(existingHoney, session);
			});
			return Ok("Honey was updated");
		}
		private Statistics CalculateStatistics(IList<Honey> honeyList)
		{
			Statistics statistics = new Statistics();
			statistics.TotalEntities = honeyList.Count;
			statistics.MeanCS = honeyList.Average(honey => honey.CS);
			statistics.MeanDensity = honeyList.Average(honey => honey.Density);
			statistics.MeanWC = honeyList.Average(honey => honey.WC);
			statistics.MeanpH = honeyList.Average(honey => honey.pH);
			statistics.MeanEC = honeyList.Average(honey => honey.EC);
			statistics.MeanF = honeyList.Average(honey => honey.F);
			statistics.MeanG = honeyList.Average(honey => honey.G);
			statistics.MeanViscosity = honeyList.Average(honey => honey.Viscosity);
			statistics.MeanPurity = honeyList.Average(honey => honey.Purity);
			statistics.MedianCS = honeyList.Median(honey => honey.CS);
			statistics.MedianDensity = honeyList.Median(honey => honey.Density);
			statistics.MedianWC = honeyList.Median(honey => honey.WC);
			statistics.MedianpH = honeyList.Median(honey => honey.pH);
			statistics.MedianEC = honeyList.Median(honey => honey.EC);
			statistics.MedianF = honeyList.Median(honey => honey.F);
			statistics.MedianG = honeyList.Median(honey => honey.G);
			statistics.MedianViscosity = honeyList.Median(honey => honey.Viscosity);
			statistics.MedianPurity = honeyList.Median(honey => honey.Purity);
			statistics.ModeCS = honeyList.Mode(honey => honey.CS);
			statistics.ModeDensity = honeyList.Mode(honey => honey.Density);
			statistics.ModeWC = honeyList.Mode(honey => honey.WC);
			statistics.ModepH = honeyList.Mode(honey => honey.pH);
			statistics.ModeEC = honeyList.Mode(honey => honey.EC);
			statistics.ModeF = honeyList.Mode(honey => honey.F);
			statistics.ModeG = honeyList.Mode(honey => honey.G);
			statistics.ModeViscosity = honeyList.Mode(honey => honey.Viscosity);
			statistics.ModePurity = honeyList.Mode(honey => honey.Purity);
			statistics.MaxCS = honeyList.Max(honey => honey.CS);
			statistics.MaxDensity = honeyList.Max(honey => honey.Density);
			statistics.MaxWC = honeyList.Max(honey => honey.WC);
			statistics.MaxpH = honeyList.Max(honey => honey.pH);
			statistics.MaxEC = honeyList.Max(honey => honey.EC);
			statistics.MaxF = honeyList.Max(honey => honey.F);
			statistics.MaxG = honeyList.Max(honey => honey.G);
			statistics.MaxViscosity = honeyList.Max(honey => honey.Viscosity);
			statistics.MaxPurity = honeyList.Max(honey => honey.Purity);
			statistics.MinCS = honeyList.Min(honey => honey.CS);
			statistics.MinDensity = honeyList.Min(honey => honey.Density);
			statistics.MinWC = honeyList.Min(honey => honey.WC);
			statistics.MinpH = honeyList.Min(honey => honey.pH);
			statistics.MinEC = honeyList.Min(honey => honey.EC);
			statistics.MinF = honeyList.Min(honey => honey.F);
			statistics.MinG = honeyList.Min(honey => honey.G);
			statistics.MinViscosity = honeyList.Min(honey => honey.Viscosity);
			statistics.MinPurity = honeyList.Min(honey => honey.Purity);
			return statistics;
		}
		private async Task UpdateHoney(HoneyDto honeyDto, Honey existingHoney)
		{

			existingHoney.CS = honeyDto.CS;
			existingHoney.Density = honeyDto.Density;
			existingHoney.WC = honeyDto.WC;
			existingHoney.pH = honeyDto.pH;
			existingHoney.EC = honeyDto.EC;
			existingHoney.F = honeyDto.F;
			existingHoney.G = honeyDto.G;
			existingHoney.Viscosity = honeyDto.Viscosity;
			existingHoney.Purity = honeyDto.Purity;
			existingHoney.PollenAnalysis = await transactionCoordinator.InCommitScopeAsync(async session =>
			{
				var pollen = await pollenRepository.GetPollenByName(honeyDto.Pollen_analysis ?? "", session);
				if (pollen == null)
				{
					pollen = new Pollen() { Name = honeyDto.Pollen_analysis };
					await pollenRepository.Insert(pollen, session);
				}
				return pollen;
			});
		}
		private async Task<Honey> ToEntity(HoneyDto honeyDto)
		{
			return new Honey
			{
				CS = honeyDto.CS,
				Density = honeyDto.Density,
				WC = honeyDto.WC,
				pH = honeyDto.pH,
				EC = honeyDto.EC,
				F = honeyDto.F,
				G = honeyDto.G,
				Viscosity = honeyDto.Viscosity,
				Purity = honeyDto.Purity,
				PollenAnalysis = await transactionCoordinator.InCommitScopeAsync(async session => 
				{
					var pollen = await pollenRepository.GetPollenByName(honeyDto.Pollen_analysis ?? "", session);
					if (pollen == null) { 
						pollen = new Pollen() { Name = honeyDto.Pollen_analysis }; 
						await pollenRepository.Insert(pollen, session);
					}
					return pollen;
				})
			};
		}

	}
	public static class StatisticExtenstion
	{

		public static float? Median(this IList<Honey> honeyList, Func<Honey, float?> selector)
		{
			var orderedList = honeyList
		.Select(selector)
		.Where(x => x.HasValue)
		.Select(x => x.Value)
		.OrderByDescending(x => x)
		.ToList();
			if (orderedList.Count == 0)
			{
				return null;
			}
			int count = orderedList.Count;
			if (count % 2 == 0)
			{
				return (orderedList[count / 2 - 1] + orderedList[count / 2]) / 2;
			}
			else
			{
				return orderedList[count / 2];
			}
		}
		public static float? Average(this IList<Honey> honeyList, Func<Honey, float?> selector)
		{
			var list = honeyList.Select(selector).Where(x => x.HasValue).Select(x=>x.Value).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			return list.Average();
		}

		public static float? Mode(this IList<Honey> honeyList, Func<Honey, float?> selector)
		{
			var list = honeyList.Select(selector).Where(x => x.HasValue).Select(x=>x.Value).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			var grouped = list.GroupBy(x => x).OrderByDescending(x => x.Count());
			return grouped.FirstOrDefault()?.Key;
		}
	}
}
