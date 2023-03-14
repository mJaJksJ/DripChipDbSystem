using System.Linq;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalVisitedLocationService
{
    /// <summary>
    /// Сервис проверок
    /// </summary>
    public class AnimalVisitedLocationEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalVisitedLocationEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Убедиться, что точка локации, которую посетило животное, существует
        /// </summary>
        public async Task<AnimalVisitedLocation> EnsureAnimalVisitedLocationExistsAsync(long animalId, long animalVisitedLocationId)
        {
            var animalVisitedLocation = await _databaseContext.AnimalVisitedLocations
                .SingleOrDefaultAsync(x => x.AnimalId == animalId && x.Id == animalVisitedLocationId);
            return animalVisitedLocation ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Убедиться, что точка локации, которую посетило животное, существует
        /// </summary>
        public AnimalVisitedLocation EnsureAnimalVisitedLocationExists(Animal animal, long animalVisitedLocationId)
        {
            var animalVisitedLocation = animal.VisitedLocations
                .SingleOrDefault(x => x.Id == animalVisitedLocationId);
            return animalVisitedLocation ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Первая точка не является точкой чипизации
        /// </summary>
        public void EnsureFirstNotChipping(
            Animal animal,
            LocationPoint location,
            AnimalVisitedLocation animalVisitedLocation)
        {
            var firstPoint = animal.VisitedLocations.FirstOrDefault();
            if (animal.ChippingLocationPointId == location.Id && firstPoint?.Id == animalVisitedLocation.Id)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Последняя точка, но не единственная
        /// </summary>
        public void EnsureLastNotOne(
            Animal animal,
            AnimalVisitedLocation animalVisitedLocation)
        {
            var lastPoint = animal.VisitedLocations.LastOrDefault();
            if (animal.VisitedLocations.Count != 1 && lastPoint?.Id == animalVisitedLocation.Id)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Изменение не на предидущую, не на следующую, если они не равны
        /// </summary>
        public void EnsurePrevNextLocations(
            Animal animal,
            LocationPoint location,
            AnimalVisitedLocation animalVisitedLocation)
        {
            var locationIndex = animal.VisitedLocations.IndexOf(animalVisitedLocation);
            var prev = animal.VisitedLocations.ElementAtOrDefault(locationIndex - 1);
            var next = animal.VisitedLocations.ElementAtOrDefault(locationIndex + 1);
            if ((prev?.LocationPointId == location.Id || next?.LocationPointId == location.Id) && prev?.LocationPointId != next?.LocationPointId)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Последняя точка не точка чиирования, если есть другие
        /// </summary>
        public void EnsureLastNotChippingWithAny(
            Animal animal,
            LocationPoint location)
        {
            if (animal.VisitedLocations.LastOrDefault()?.LocationPointId == location.Id ||
                !animal.VisitedLocations.Any() && animal.ChippingLocationPointId == location.Id)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
