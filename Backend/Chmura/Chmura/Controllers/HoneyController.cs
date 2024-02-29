using Chmura.Database.Entities;
using Chmura.Dto;
using Chmura.ORM;
using Chmura.Repository;
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

		public HoneyController(ILogger<HoneyController> logger, ITransactionCoordinator transactionCoordinator, IHoneyRepository honeyRepository)
		{
			this.transactionCoordinator = transactionCoordinator;
			this.honeyRepository = honeyRepository;
			_logger = logger;
		}

		[HttpGet("GetAll", Name = "GetAll")]
		public async Task<ActionResult<IList<HoneyDto>>> GetAll()
		{
			IList<HoneyDto> result = new List<HoneyDto> ();
			await transactionCoordinator.InRollbackScopeAsync(async session =>
			{
				var honeyList = await honeyRepository.GetAll(session);
				result = honeyList.Select(honey => honey.ToDto()).ToList();
			});
			return Ok(result);
		}
			
	}
}
