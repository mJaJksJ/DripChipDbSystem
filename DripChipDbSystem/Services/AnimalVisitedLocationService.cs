using DripChipDbSystem.Api.Controllers.AnimalController;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using DripChipDbSystem.Api.Controllers.AnimalVisitedLocation;
using DripChipDbSystem.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Security.Cryptography.X509Certificates;

namespace DripChipDbSystem.Services
{
    public class AnimalVisitedLocationService
    {
        private readonly DatabaseContext _databaseContext;

        public AnimalVisitedLocationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<AnimalVisitedLocationResponseContract>> SearchAsync(
            long animalId,
            DateTime startDateTime,
            DateTime endDateTime,
            int from,
            int size)
        {
            return await _databaseContext.AnimalVisitedLocations
                .AsNoTracking()
                .Where(x =>
                    x.AnimalId == animalId &&
                    x.VisitedDateTime >= startDateTime &&
                    x.VisitedDateTime <= endDateTime
                )
                .Skip(from)
                .Take(size)
                .Select(x => new AnimalVisitedLocationResponseContract
                {
                    Id = x.Id,
                    LocationPointId = x.LocationPointId,
                    DateTimeOfVisitLocationPoint = x.VisitedDateTime
                })
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<AnimalVisitedLocationResponseContract> AddAnimalVisitedLocationAsync(long animalId, long pointId)
        {
            var newAnimalVisitedLocation = new AnimalVisitedLocation
            {
                AnimalId = animalId,
                LocationPointId = pointId
            };

            await _databaseContext.AddAsync(newAnimalVisitedLocation);
            await _databaseContext.SaveChangesAsync();

            return new AnimalVisitedLocationResponseContract
            {
                Id = newAnimalVisitedLocation.Id,
                LocationPointId = newAnimalVisitedLocation.LocationPointId,
                DateTimeOfVisitLocationPoint = newAnimalVisitedLocation.VisitedDateTime
            };
        }

        public async Task<AnimalVisitedLocationResponseContract> UpdateAnimalVisitedLocationAsync(long animalId, AnimalVisitedLocationRequestContract contract)
        {
            var animalVisitedLocation = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.AnimalId == animalId && x.Id == contract.VisitedLocationPointId);

            animalVisitedLocation.LocationPointId = contract.LocationPointId;

            await _databaseContext.SaveChangesAsync();
            return new AnimalVisitedLocationResponseContract
            {
                Id = animalVisitedLocation.Id,
                LocationPointId = animalVisitedLocation.LocationPointId,
                DateTimeOfVisitLocationPoint = animalVisitedLocation.VisitedDateTime
            };
        }

        public async Task DeleteAnimalVisitedLocationAsync(long animalId, long visitedPointId)
        {
            var animal = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.Id == animalId && x.LocationPointId == visitedPointId);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

    }
}
