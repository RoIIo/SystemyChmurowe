
using Chmura.Domain;
using Chmura.ORM;
using Chmura.Repository;

namespace Chmura
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<INHibernateHelper, NHibernateHelper>();
			builder.Services.AddSingleton<ITransactionCoordinator, TransactionCoordinator>();
			builder.Services.AddSingleton<IHoneyRepository, HoneyRepository>();
			builder.Services.AddSingleton<IPollenRepository, PollenRepository>();
			builder.Services.AddSingleton<ICSVReader, CSVReader>();

			var app = builder.Build();

			var reader = app.Services.GetRequiredService<ICSVReader>();
			string? dataPath = builder.Configuration.GetSection("DataPath").Value.ToString();
			reader.LoadData(dataPath);

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
