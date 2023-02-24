using Microsoft.Extensions.DependencyInjection;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Расширения для сервисов
    /// </summary>
    public static class ServicesExtensions
    {
        /// <summary>
        /// Добавить сервисы
        /// </summary>
        public static IServiceCollection AddDripChipServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<AccountService>();
            services.AddScoped<LocationService>();
            services.AddScoped<AnimalTypeService>();
            services.AddScoped<AnimalService>();
            services.AddScoped<AnimalVisitedLocationService>();
            return services;
        }
    }
}
