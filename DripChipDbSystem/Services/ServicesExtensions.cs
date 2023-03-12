using DripChipDbSystem.Services.AccountService;
using DripChipDbSystem.Services.AnimalService;
using DripChipDbSystem.Services.AnimalTypeService;
using DripChipDbSystem.Services.AnimalVisitedLocationService;
using DripChipDbSystem.Services.LocationService;
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
            services.AddScoped<AnimalTypeEnsureService>();
            services.AddScoped<LocationEnsureService>();
            services.AddScoped<AnimalEnsureService>();
            services.AddScoped<LocationEnsureService>();
            services.AddScoped<AccountEnsureService>();
            services.AddScoped<AnimalVisitedLocationEnsureService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AccountService.AccountService>();
            services.AddScoped<LocationService.LocationService>();
            services.AddScoped<AnimalTypeService.AnimalTypeService>();
            services.AddScoped<AnimalService.AnimalService>();
            services.AddScoped<AnimalVisitedLocationService.AnimalVisitedLocationService>();
            return services;
        }
    }
}
