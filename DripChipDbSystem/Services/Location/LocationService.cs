using System;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Services.Location;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с точками локации
    /// </summary>
    public class LocationService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly LocationEnsureService _locationEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public LocationService(
            DatabaseContext databaseContext,
            LocationEnsureService locationEnsureService)
        {
            _databaseContext = databaseContext;
            _locationEnsureService = locationEnsureService;
        }

        /// <summary>
        /// Получение информации о точке локации животных
        /// </summary>
        public async Task<LocationResponseContract> GetLocationAsync(long pointId)
        {
            var location = await _databaseContext.LocationPoints
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == pointId);

            return location is null
                ? throw new NotFound404Exception()
                : new LocationResponseContract(location);
        }

        /// <summary>
        /// Добавление точки локации животных
        /// </summary>
        public async Task<LocationResponseContract> AddLocationAsync(LocationRequestContract contract)
        {
            await _locationEnsureService.EnsureLocationNotExists(contract);
            var newLocation = new LocationPoint
            {
                Latitude = contract.Latitude.GetValueOrDefault(),
                Longitude = contract.Longitude.GetValueOrDefault()
            };

            await _databaseContext.LocationPoints
                .AddAsync(newLocation);
            await _databaseContext.SaveChangesAsync();
            return new LocationResponseContract(newLocation);
        }

        /// <summary>
        /// Изменение точки локации животных
        /// </summary>
        public async Task<LocationResponseContract> UpdateLocationAsync(long pointId, LocationRequestContract contract)
        {
            await _locationEnsureService.EnsureLocationNotExists(contract);
            var location = await _locationEnsureService.EnsureLocationExists(pointId);

            location.Latitude = contract.Latitude.GetValueOrDefault();
            location.Longitude = contract.Longitude.GetValueOrDefault();

            await _databaseContext.SaveChangesAsync();
            return new LocationResponseContract(location);
        }

        /// <summary>
        /// Удаление точки локации животных 
        /// </summary>
        public async Task DeleteLocationAsync(long pointId)
        {
            await _locationEnsureService.EnsureLocationNotChippingPoint(pointId);
            await _locationEnsureService.EnsureLocationNotVisited(pointId);
            var location = await _locationEnsureService.EnsureLocationExists(pointId);
            _databaseContext.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }
    }
}

