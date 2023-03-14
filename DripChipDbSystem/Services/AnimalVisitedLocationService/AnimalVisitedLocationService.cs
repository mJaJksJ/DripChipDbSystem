using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalVisitedLocation;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Services.AnimalService;
using DripChipDbSystem.Services.LocationService;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalVisitedLocationService
{
    /// <summary>
    /// Сервис работы с точками локации, которые посетило животное
    /// </summary>
    public class AnimalVisitedLocationService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly LocationEnsureService _locationEnsureService;
        private readonly AnimalEnsureService _animalEnsureService;
        private readonly AnimalVisitedLocationEnsureService _animalVisitedLocationEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalVisitedLocationService(
            DatabaseContext databaseContext,
            LocationEnsureService locationEnsureService,
            AnimalEnsureService animalEnsureService,
            AnimalVisitedLocationEnsureService animalVisitedLocationEnsureService)
        {
            _databaseContext = databaseContext;
            _locationEnsureService = locationEnsureService;
            _animalEnsureService = animalEnsureService;
            _animalVisitedLocationEnsureService = animalVisitedLocationEnsureService;
        }

        /// <summary>
        /// Просмотр точек локации, посещенных животным
        /// </summary>
        public async Task<IEnumerable<AnimalVisitedLocationResponseContract>> SearchAsync(
            long animalId,
            DateTimeOffset? startDateTime,
            DateTimeOffset? endDateTime,
            int? from,
            int? size)
        {
            return (await _databaseContext.AnimalVisitedLocations
                .AsNoTracking()
                .Where(x =>
                    x.AnimalId == animalId &&
                    (startDateTime == null || x.VisitedDateTime >= startDateTime) &&
                    (endDateTime == null || x.VisitedDateTime <= endDateTime)
                )
                .OrderBy(x => x.Id)
                .Skip(from ?? 0)
                .Take(size ?? 10)
                .ToListAsync())
                .Select(x => new AnimalVisitedLocationResponseContract(x));
        }

        /// <summary>
        /// Добавление точки локации, посещенной животным
        /// </summary>
        public async Task<AnimalVisitedLocationResponseContract> AddAnimalVisitedLocationAsync(long animalId, long pointId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            _animalEnsureService.EnsureAnimalNotDead(animal);
            var location = await _locationEnsureService.EnsureLocationExistsAsync(pointId);
            _animalVisitedLocationEnsureService.EnsureLastNotChippingWithAny(animal, location);

            var newAnimalVisitedLocation = await _databaseContext.AddAsync(new AnimalVisitedLocation
            {
                Animal = animal,
                LocationPoint = location,
                VisitedDateTime = DateTimeOffset.Now
            });
            await _databaseContext.SaveChangesAsync();

            return new AnimalVisitedLocationResponseContract(await _databaseContext.AnimalVisitedLocations.SingleAsync(x => x.Id == newAnimalVisitedLocation.Entity.Id));
        }

        /// <summary>
        /// Изменение точки локации, посещенной животным
        /// </summary>
        public async Task<AnimalVisitedLocationResponseContract> UpdateAnimalVisitedLocationAsync(long animalId, AnimalVisitedLocationRequestContract contract)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            var animalVisitedLocation = await _animalVisitedLocationEnsureService.EnsureAnimalVisitedLocationExistsAsync(animalId, contract.VisitedLocationPointId.GetValueOrDefault());
            var location = await _locationEnsureService.EnsureLocationExistsAsync(contract.LocationPointId.GetValueOrDefault());
            _animalVisitedLocationEnsureService.EnsureFirstNotChipping(animal, location, animalVisitedLocation);
            _animalVisitedLocationEnsureService.EnsureLastNotOne(animal, animalVisitedLocation);
            _animalVisitedLocationEnsureService.EnsurePrevNextLocations(animal, location, animalVisitedLocation);

            animalVisitedLocation.LocationPoint = location;
            await _databaseContext.SaveChangesAsync();
            return new AnimalVisitedLocationResponseContract(animalVisitedLocation);
        }

        /// <summary>
        /// Удаление точки локации, посещенной животным
        /// </summary>
        public async Task DeleteAnimalVisitedLocationAsync(long animalId, long visitedPointId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            var location = _animalVisitedLocationEnsureService.EnsureAnimalVisitedLocationExists(animal, visitedPointId);

            if (animal.VisitedLocations.Count >= 2)
            {
                var first = animal.VisitedLocations[0];
                var second = animal.VisitedLocations[1];
                if (first.Id == location.Id && second.LocationPointId == animal.ChippingLocationPointId)
                {
                    animal.VisitedLocations.Remove(second);
                }
            }
            animal.VisitedLocations.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
