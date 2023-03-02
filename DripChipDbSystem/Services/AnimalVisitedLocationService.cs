using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalVisitedLocation;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с точками локации, которые посетило животное
    /// </summary>
    public class AnimalVisitedLocationService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalVisitedLocationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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
            var newAnimalVisitedLocation = new AnimalVisitedLocation
            {
                AnimalId = animalId,
                LocationPointId = pointId
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
            var animalVisitedLocation = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.AnimalId == animalId && x.Id == contract.VisitedLocationPointId);

            animalVisitedLocation.LocationPointId = contract.LocationPointId.GetValueOrDefault();

            await _databaseContext.SaveChangesAsync();
            return new AnimalVisitedLocationResponseContract(animalVisitedLocation);
        }

        /// <summary>
        /// Удаление точки локации, посещенной животным
        /// </summary>
        public async Task DeleteAnimalVisitedLocationAsync(long animalId, long visitedPointId)
        {
            var animal = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.Id == animalId && x.LocationPointId == visitedPointId);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

    }
}
