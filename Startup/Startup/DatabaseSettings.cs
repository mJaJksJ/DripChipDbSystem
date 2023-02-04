using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Startup.Configuration;

namespace Startup.Startup
{
    public static class DatabaseSettings
    {
        public static void AdddDatabaseService<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<TContext>(
                    options =>
                    {
                        options.UseNpgsql(GetConnectionString(configuration), x => x.MigrationsAssembly("DripChipDbSystem"));
                        options.EnableSensitiveDataLogging();
                    });
        }

        public static void UseDatabase<TContext>(this IHost app)
            where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Database.Migrate();

            var npgSqlConnection = context.Database.GetDbConnection() as NpgsqlConnection;
            npgSqlConnection?.Open();
            npgSqlConnection?.ReloadTypes();
            npgSqlConnection?.Close();
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var config = configuration.GetSection(DatabaseConfig.ConfigName).Get<DatabaseConfig>();
            var sb = new StringBuilder("");
            sb.Append($"Server={config.Server};");
            sb.Append($"Port={config.Port};");
            sb.Append($"Database={config.DatabaseName};");
            sb.Append($"User Id={config.User};");
            sb.Append($"Password={config.Password};");
            return sb.ToString();
        }
    }
}
