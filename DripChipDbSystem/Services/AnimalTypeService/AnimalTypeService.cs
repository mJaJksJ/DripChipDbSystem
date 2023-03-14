using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalTypeController;
using DripChipDbSystem.Database;

namespace DripChipDbSystem.Services.AnimalTypeService
{
    /// <summary>
    /// Сервис работы типами животных
    /// </summary>
    public class AnimalTypeService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AnimalTypeEnsureService _animalTypeEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalTypeService(
            DatabaseContext databaseContext,
            AnimalTypeEnsureService animalTypeEnsureService)
        {
            _databaseContext = databaseContext;
            _animalTypeEnsureService = animalTypeEnsureService;
        }

        /// <summary>
        /// Получение информации о типе животного
        /// </summary>
        public async Task<AnimalTypeResponseContract> GetAnimalTypeAsync(long typeId)
        {
            var animalType = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(typeId);
            return new AnimalTypeResponseContract(animalType);
        }

        /// <summary>
        /// Добавление типа животного
        /// </summary>
        public async Task<AnimalTypeResponseContract> AddAnimalTypeAsync(AnimalTypeRequestContract contract)
        {
            await _animalTypeEnsureService.EnsureAnimalTypeNotExistsAsync(contract);
            var newAnimalType = new Database.Models.Animals.AnimalType
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
            await _animalTypeEnsureService.EnsureAnimalTypeNotExistsAsync(contract);
            var animalType = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(typeId);

            animalType.Type = contract.Type;

            await _databaseContext.SaveChangesAsync();
            return new AnimalTypeResponseContract(animalType);
        }

        /// <summary>
        /// Удаление типа животного
        /// </summary>
        public async Task DeleteAnimalTypeAsync(long typeId)
        {
            await _animalTypeEnsureService.EnsureTypeHasNoAnimalsAsync(typeId);
            var animalType = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(typeId);

            _databaseContext.Remove(animalType);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
