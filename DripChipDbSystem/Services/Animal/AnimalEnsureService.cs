using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.Animal
{
    public class AnimalEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        public AnimalEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        /// <summary>
        /// Убедиться, что животное существует
        /// </summary>
        public async Task<Database.Models.Animals.Animal> EnsureAnimalExists(long id)
        {
            var animal = await _databaseContext.Animals
                .Include(x => x.VisitedLocations)
                .Include(x => x.AnimalTypes)
                .SingleOrDefaultAsync(x => x.Id == id);

            return animal ?? throw new NotFound404Exception();
        }

        public async Task EnsureAnimalTypesExistAsync(IEnumerable<long?> animalTypeIds)
        {
            var animalTypes = await _databaseContext.AnimalTypes
                .Select(x => x.Id)
                .ToListAsync();

            var allExists = animalTypeIds.All(x => x.HasValue && animalTypes.Contains(x.Value));

            if (!allExists)
            {
                throw new NotFound404Exception();
            }
        }

        public async Task<Database.Models.Auth.Account> EnsureChiperExistsAsync(int? chipperId)
        {
            var chipper = await _databaseContext.Accounts
                .SingleOrDefaultAsync(x => x.Id == chipperId);

            return chipper ?? throw new NotFound404Exception();
        }

        public async Task<LocationPoint> EnsureChippingLocationExistsAsync(long? locationId)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == locationId);

            return location ?? throw new NotFound404Exception();
        }

        public void EnsureAnimalTypesNotRepeated(AddingAnimalRequestContract contract)
        {
            var animalTypes = contract.AnimalTypes as long?[] ?? contract.AnimalTypes.ToArray();
            if (animalTypes is not null &&
                animalTypes.Distinct().Count() != animalTypes.Length)
            {
                throw new Conflict409Exception();
            }
        }

        public void EnsureAnimalLeftChippingLocationButHasOther(Database.Models.Animals.Animal animal)
        {
            if (animal.VisitedLocations.Any() &&
                animal.VisitedLocations.Last().Id != animal.ChippingLocationPointId)
            {
                throw new BadRequest400Exception();
            }
        }

        public Database.Models.Animals.AnimalType EnsureAnimalHasType(long typeId, Database.Models.Animals.Animal animal)
        {
            var animalType = animal.AnimalTypes.SingleOrDefault(x => x.Id == typeId);
            return animalType ?? throw new NotFound404Exception();
        }

        public void EnsureAnimalHasNotType(long typeId, Database.Models.Animals.Animal animal)
        {
            var isExists = animal.AnimalTypes.Any(x => x.Id == typeId);
            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        public void EnsureNotLastType(Database.Models.Animals.Animal animal)
        {
            if (animal.AnimalTypes.Count == 1)
            {
                throw new BadRequest400Exception();
            }
        }

        public void EnsureAnimalNotDead(Database.Models.Animals.Animal animal)
        {
            var isDead = animal.LifeStatus == LifeStatus.Dead;
            if (isDead)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
