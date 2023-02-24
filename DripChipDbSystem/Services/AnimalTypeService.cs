using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalTypeController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы типами животных
    /// </summary>
    public class AnimalTypeService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalTypeService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Получение информации о типе животного
        /// </summary>
        public async Task<AnimalTypeResponseContract> GetAnimalTypeAsync(long typeId)
        {
            var animalType = await _databaseContext.AnimalTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == typeId);

            return animalType is null
                ? throw new NotFound404Exception()
                : new AnimalTypeResponseContract(animalType);
        }

        /// <summary>
        /// Добавление типа животного
        /// </summary>
        public async Task<AnimalTypeResponseContract> AddAnimalTypeAsync(AnimalTypeRequestContract contract)
        {
            await EnsureAnimalTypeNotExists(contract);
            var newAnimalType = new AnimalType
            {
                Type = contract.Type,
            };
            await _databaseContext.AnimalTypes.AddAsync(newAnimalType);
            await _databaseContext.SaveChangesAsync();
            return new AnimalTypeResponseContract(newAnimalType);
        }

        /// <summary>
        /// Изменение типа животного
        /// </summary>
        public async Task<AnimalTypeResponseContract> UpdateAnimalTypeAsync(long typeId, AnimalTypeRequestContract contract)
        {
            var animalType = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

            animalType.Type = contract.Type;

            await _databaseContext.SaveChangesAsync();
            return new AnimalTypeResponseContract(animalType);
        }

        /// <summary>
        /// Удаление типа животного
        /// </summary>
        public async Task DeleteAnimalTypeAsync(long typeId)
        {
            var animalType = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

            _databaseContext.Remove(animalType);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Убедиться, что типа животного не существует
        /// </summary>
        public async Task EnsureAnimalTypeNotExists(AnimalTypeRequestContract contract)
        {
            var isExists = await _databaseContext.AnimalTypes
                .AnyAsync(x => x.Type == contract.Type);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }
    }
}
