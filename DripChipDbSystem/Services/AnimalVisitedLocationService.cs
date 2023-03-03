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
            DateTime startDateTime,
            DateTime endDateTime,
            int? from,
            int? size)
        {
            return (await _databaseContext.AnimalVisitedLocations
                .AsNoTracking()
                .Where(x =>
                    x.AnimalId == animalId &&
                    x.VisitedDateTime >= startDateTime &&
                    x.VisitedDateTime <= endDateTime
                )
                .Skip(from ?? 0)
                .Take(size ?? 10)
                .OrderBy(x => x.Id)
                .ToListAsync())
                .Select(x => new AnimalVisitedLocationResponseContract(x));
        }

        /// <summary>
        /// Добавление точки локации, посещенной животным
        /// </summary>
        public async Task<AnimalVisitedLocationResponseContract> AddAnimalVisitedLocationAsync(long animalId, long pointId)
        {
            await EnshureLocationNotVisited(animalId, pointId);
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            _animalEnsureService.EnsureAnimalNotDead(animal);
            var location = await _locationEnsureService.EnsureLocationExists(pointId);

            if (animal.VisitedLocations.LastOrDefault()?.LocationPointId == location.Id)
            {
                throw new BadRequest400Exception();
            }

            var newAnimalVisitedLocation = new AnimalVisitedLocation
            {
                Animal = animal,
                LocationPoint = location,
                VisitedDateTime = DateTimeOffset.Now
            };

            await _databaseContext.AddAsync(newAnimalVisitedLocation);
            await _databaseContext.SaveChangesAsync();

            return new AnimalVisitedLocationResponseContract(newAnimalVisitedLocation);
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
            if (firstPoint?.Id == animalVisitedLocation.Id && animal.ChippingLocationPointId == contract.LocationPointId)
            {
                throw new BadRequest400Exception();
            }

            var lastPoint = animal.VisitedLocations.LastOrDefault();
            if (lastPoint?.Id == animalVisitedLocation.Id)
            {
                throw new BadRequest400Exception();
            }

            var locationIndex = animal.VisitedLocations.IndexOf(animalVisitedLocation);
            var prev = animal.VisitedLocations.ElementAtOrDefault(locationIndex - 1);
            var next = animal.VisitedLocations.ElementAtOrDefault(locationIndex + 1);
            if (prev?.LocationPointId == location.Id ^ next?.LocationPointId == location.Id)
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
                if (first.Id == location.Id && second.Id == animal.ChippingLocationPointId)
                {
                    animal.VisitedLocations.Remove(second);
                }
            }
            animal.VisitedLocations.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }

        private async Task EnshureLocationNotVisited(long animalId, long pointId)
        {
            var isVisited = await _databaseContext.AnimalVisitedLocations
                .AnyAsync(x => x.AnimalId == animalId && x.LocationPointId == pointId);

            if (isVisited)
            {
                throw new BadRequest400Exception();
            }
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
