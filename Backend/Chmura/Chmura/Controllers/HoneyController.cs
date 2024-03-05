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

		[HttpGet("GetMetadata", Name = "GetMetadata")]
		public async Task<ActionResult<Metadata>> GetMetadata([FromQuery] HoneyFilter filter)
		{
			Metadata result = new Metadata();
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				result.TotalEntities = await honeyRepository.GetTotalEntities(filter, session);
			});
			return Ok(result);
		}

		[HttpGet("GetAll", Name = "GetAll")]
        public async Task<ActionResult<IList<HoneyDto>>> GetAll([FromQuery] int page = 0, [FromQuery] int pageSize = 100, [FromQuery] HoneyFilter? filter = null)
		{
			IList<HoneyDto> result = new List<HoneyDto> ();
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				var honeyList = await honeyRepository.GetPage(page, pageSize, filter, session);
				result = honeyList.Select(honey => honey.ToDto()).ToList();
			});
			return Ok(result);
		}

		[HttpPost("AddHoney", Name= "AddHoney")]
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
			existingHoney.PollenAnalysis = await transactionCoordinator.InRollbackScopeAsync(async session => await pollenRepository.GetPollenByName(honeyDto.Pollen_analysis ?? "", session));
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
				PollenAnalysis = await transactionCoordinator.InRollbackScopeAsync(async session => await pollenRepository.GetPollenByName(honeyDto.Pollen_analysis ?? "", session))
			};
		}
			
	}
}
