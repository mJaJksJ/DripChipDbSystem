using DripChipDbSystem.Authentification;
using DripChipDbSystem.Database;
using DripChipDbSystem.Middlewares.BasicLogOutMiddleware;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using DripChipDbSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Startup.Startup;
using Startup.SwaggerSettings;

namespace DripChipDbSystem
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(options => options.DefaultScheme = BasicAuth.Scheme)
                .AddScheme<BasicAuthSchemeOptions, BasicAuthHandler>(BasicAuth.Scheme, _ => { });
            builder.Services.AdddDatabaseService<DatabaseContext>(builder.Configuration);
            builder.Services.AddSwaggerService();

            builder.Services.AddDripChipServices();

            var app = builder.Build();

            app.UseBasicLogOutMiddlewareExtensions();
            app.UseHttpResponseMiddleware();
            app.UseDatabase<DatabaseContext>();

            app.UseSwaggerMiddleware();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}