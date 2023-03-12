using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalVisitedLocation;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Services.Animal;
using DripChipDbSystem.Services.Location;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с точками локации, которые посетило животное
    /// </summary>
    public class AnimalVisitedLocationService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly LocationEnsureService _locationEnsureService;
        private readonly AnimalEnsureService _animalEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalVisitedLocationService(
            DatabaseContext databaseContext,
            LocationEnsureService locationEnsureService,
            AnimalEnsureService animalEnsureService)
        {
            _databaseContext = databaseContext;
            _locationEnsureService = locationEnsureService;
            _animalEnsureService = animalEnsureService;
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
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            _animalEnsureService.EnsureAnimalNotDead(animal);
            var location = await _locationEnsureService.EnsureLocationExists(pointId);

            if (animal.VisitedLocations.LastOrDefault()?.LocationPointId == location.Id ||
                (!animal.VisitedLocations.Any() && animal.ChippingLocationPointId == location.Id))
            {
                throw new BadRequest400Exception();
            }

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
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            var animalVisitedLocation = await EnsureAnimalVisitedLocationExists(animalId, contract.VisitedLocationPointId.GetValueOrDefault());
            var location = await _locationEnsureService.EnsureLocationExists(contract.LocationPointId.GetValueOrDefault());

            var firstPoint = animal.VisitedLocations.FirstOrDefault();
            if (animal.ChippingLocationPointId == location.Id && firstPoint?.Id == animalVisitedLocation.Id)
            {
                throw new BadRequest400Exception();
            }

            var lastPoint = animal.VisitedLocations.LastOrDefault();
            if (animal.VisitedLocations.Count != 1 && lastPoint?.Id == animalVisitedLocation.Id)
            {
                throw new BadRequest400Exception();
            }

            var locationIndex = animal.VisitedLocations.IndexOf(animalVisitedLocation);
            var prev = animal.VisitedLocations.ElementAtOrDefault(locationIndex - 1);
            var next = animal.VisitedLocations.ElementAtOrDefault(locationIndex + 1);
            if ((prev?.LocationPointId == location.Id || next?.LocationPointId == location.Id) && prev?.LocationPointId != next?.LocationPointId)
            {
                throw new BadRequest400Exception();
            }

            animalVisitedLocation.LocationPoint = location;
            await _databaseContext.SaveChangesAsync();
            return new AnimalVisitedLocationResponseContract(animalVisitedLocation);
        }

        /// <summary>
        /// Удаление точки локации, посещенной животным
        /// </summary>
        public async Task DeleteAnimalVisitedLocationAsync(long animalId, long visitedPointId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            var location = await EnsureAnimalVisitedLocationExists(animal, visitedPointId);

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

        public async Task<AnimalVisitedLocation> EnsureAnimalVisitedLocationExists(long animalId, long animalVisitedLocationId)
        {
            var animalVisitedLocation = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.AnimalId == animalId && x.Id == animalVisitedLocationId);
            return animalVisitedLocation ?? throw new NotFound404Exception();
        }

        public async Task<AnimalVisitedLocation> EnsureAnimalVisitedLocationExists(Database.Models.Animals.Animal animal, long animalVisitedLocationId)
        {
            var animalVisitedLocation = animal.VisitedLocations
                .SingleOrDefault(x => x.Id == animalVisitedLocationId);
            return animalVisitedLocation ?? throw new NotFound404Exception();
        }
    }
}
