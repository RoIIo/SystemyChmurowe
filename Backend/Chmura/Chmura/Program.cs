
using Chmura.Domain;
using Chmura.ORM;
using Chmura.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Chmura
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var specificOrgins = "AppOrigins";

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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            //app.UseCors(specificOrgins);
            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
