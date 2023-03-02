using System;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с точками локации
    /// </summary>
    public class LocationService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public LocationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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
            await EnsureLocationNotExists(contract);
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
            await EnsureLocationNotExists(contract);
            var location = await EnsureLocationExists(pointId);

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
            await EnsureLocationNotChippingPoint(pointId);
            await EnsureLocationNotVisited(pointId);
            var location = await EnsureLocationExists(pointId);
            _databaseContext.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }

        private async Task EnsureLocationNotExists(LocationRequestContract contract)
        {
            const float epsilon = 0.000001f;
            var isExists = await _databaseContext.LocationPoints
                .AnyAsync(x => Math.Abs(x.Latitude - contract.Latitude.Value) < epsilon &&
                    Math.Abs(x.Longitude - contract.Longitude.Value) < epsilon);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        private async Task<LocationPoint> EnsureLocationExists(long pointId)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == pointId);

            return location ?? throw new NotFound404Exception();
        }

        private async Task EnsureLocationNotChippingPoint(long pointId)
        {
            var isChippingPoint = await _databaseContext.Animals
                .AnyAsync(x => x.ChippingLocationPointId == pointId);

            if (isChippingPoint)
            {
                throw new BadRequest400Exception();
            }
        }
        private async Task EnsureLocationNotVisited(long pointId)
        {
            var isChippingPoint = await _databaseContext.Animals
                .AnyAsync(x => x.VisitedLocations
                    .Any(vl => vl.LocationPointId == pointId));

            if (isChippingPoint)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}

