using System;
using DripChipDbSystem.Api.Controllers.AccountController;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using Microsoft.EntityFrameworkCore;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database.Models.Animals;

namespace DripChipDbSystem.Services
{
    public class LocationService
    {
        private readonly DatabaseContext _databaseContext;

        public LocationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<LocationResponseContract> GetLocationAsync(long pointId)
        {
            var location = await _databaseContext.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == pointId);

            if (location is null)
            {
                throw new NotFound404Exception() { Data = { { HttpResponseMiddleware.ResultKey, new LocationResponseContract() } } };
            }

            return new LocationResponseContract
            {
                Id = location.Id,
            };
        }

        public async Task<LocationResponseContract> AddLocationAsync(LocationRequestContract contract)
        {
            var location = await _databaseContext.LocationPoints
                .AddAsync(new LocationPoint
                {
                    Latitude = contract.Latitude.Value,
                    Longitude = contract.Longitude.Value
                });
            await _databaseContext.SaveChangesAsync();
            return new LocationResponseContract
            {
                Id = location.Entity.Id,
                Latitude = location.Entity.Latitude,
                Longitude = location.Entity.Longitude
            };
        }

        public async Task<LocationResponseContract> UpdateLocationAsync(long pointId, LocationRequestContract contract)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == pointId);

            location.Latitude = contract.Latitude.Value;
            location.Longitude = contract.Longitude.Value;

            await _databaseContext.SaveChangesAsync();
            return new LocationResponseContract
            {
                Id = location.Id,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        public async Task DeleteLocationAsync(long pointId)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == pointId);

            _databaseContext.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task EnsureLocationNotExists(LocationRequestContract contract)
        {
            var isExists = await _databaseContext.LocationPoints
                .AnyAsync(x => x.Latitude == contract.Latitude.Value && x.Longitude == contract.Longitude.Value);

            if (isExists)
            {
                throw new Conflict409Exception() { Data = { { HttpResponseMiddleware.ResultKey, new AccountResponseContract() } } };
            }
        }
    }
}

