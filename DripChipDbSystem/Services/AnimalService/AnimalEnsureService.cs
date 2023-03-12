using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalService
{
    /// <summary>
    /// Сервис проверок
    /// </summary>
    public class AnimalEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Убедиться, что животное существует
        /// </summary>
        public async Task<Animal> EnsureAnimalExistsAsync(long id)
        {
            var animal = await _databaseContext.Animals
                .Include(x => x.VisitedLocations)
                .Include(x => x.AnimalTypes)
                .SingleOrDefaultAsync(x => x.Id == id);

            return animal ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Убедиться, что типы животного не повторяются
        /// </summary>
        public void EnsureAnimalTypesNotRepeated(AddingAnimalRequestContract contract)
        {
            var animalTypes = contract.AnimalTypes as long?[] ?? contract.AnimalTypes.ToArray();
            if (animalTypes is not null &&
                animalTypes.Distinct().Count() != animalTypes.Length)
            {
                throw new Conflict409Exception();
            }
        }

        /// <summary>
        /// Убедиться, что животное покинуло точку локации и она не единственная
        /// </summary>
        public void EnsureAnimalLeftChippingLocationButHasOther(Animal animal)
        {
            if (animal.VisitedLocations.Any() &&
                animal.VisitedLocations.Last().Id != animal.ChippingLocationPointId)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Убедиться, что животное имеет тип
        /// </summary>
        public AnimalType EnsureAnimalHasType(long typeId, Animal animal)
        {
            var animalType = animal.AnimalTypes.SingleOrDefault(x => x.Id == typeId);
            return animalType ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Убедиться, что животное не имеет типа
        /// </summary>
        public void EnsureAnimalHasNotType(long typeId, Animal animal)
        {
            var isExists = animal.AnimalTypes.Any(x => x.Id == typeId);
            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        /// <summary>
        /// Убедиться, что тип не единственный
        /// </summary>
        public void EnsureNotLastType(Animal animal)
        {
            if (animal.AnimalTypes.Count == 1)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Убедиться, что животное не мертво
        /// </summary>
        public void EnsureAnimalNotDead(Animal animal)
        {
            var isDead = animal.LifeStatus == LifeStatus.Dead;
            if (isDead)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
