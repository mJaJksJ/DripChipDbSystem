using DripChipDbSystem.Authentification;
using DripChipDbSystem.Database;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Startup.Startup;

namespace DripChipDbSystem
{
#pragma warning disable CS1591
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

            builder.Services.AddDripChipServices();

            builder.Services.AddAuthentication(options => options.DefaultScheme = BasicAuth.Scheme)
                .AddScheme<BasicAuthSchemeOptions, BasicAuthHandler>(BasicAuth.Scheme, _ => { });

            var app = builder.Build();
            app.UseHttpResponseMiddleware();
            app.UseDatabase<DatabaseContext>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}