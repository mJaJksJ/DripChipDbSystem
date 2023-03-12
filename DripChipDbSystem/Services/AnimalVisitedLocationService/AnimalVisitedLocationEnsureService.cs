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
    }
}
