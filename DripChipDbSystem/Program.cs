using DripChipDbSystem.Database;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Startup.Startup;

namespace DripChipDbSystem
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

            builder.Services.AdddDatabaseService<DatabaseContext>(builder.Configuration);

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<AnimalTypeService>();
            builder.Services.AddScoped<AnimalService>();
            builder.Services.AddScoped<AnimalVisitedLocationService>();

            var app = builder.Build();
            app.UseDatabase<DatabaseContext>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseHttpResponseMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}