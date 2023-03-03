using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Services.AnimalType;
using DripChipDbSystem.Utils;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.Animal
{
    /// <summary>
    /// Сервис работы с животными
    /// </summary>
    public class AnimalService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AnimalTypeEnsureService _animalTypeEnsureService;
        private readonly AnimalEnsureService _animalEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalService(
            DatabaseContext databaseContext,
            AnimalTypeEnsureService animalTypeEnsureService,
            AnimalEnsureService animalEnsureService)
        {
            _databaseContext = databaseContext;
            _animalTypeEnsureService = animalTypeEnsureService;
            _animalEnsureService = animalEnsureService;
        }

        /// <summary>
        /// Получение информации о животном
        /// </summary>
        public async Task<AnimalResponseContract> GetAnimalAsync(long animalId)
        {
            var animal = await _databaseContext.Animals
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == animalId);

            return animal is null
                ? throw new NotFound404Exception()
                : new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Поиск животных по параметрам
        /// </summary>
        public async Task<IEnumerable<AnimalResponseContract>> SearchAsync(
            DateTime startDateTime,
            DateTime endDateTime,
            int chipperId,
            long chippingLocationId,
            string lifeStatus,
            string gender,
            int from,
            int size)
        {
            return (await _databaseContext.Animals
                .AsNoTracking()
                .Where(x =>
                    x.ChippingDateTime >= startDateTime &&
                    x.ChippingDateTime <= endDateTime &&
                    x.ChipperId == chipperId &&
                    x.ChippingLocationPointId == chippingLocationId &&
                    x.LifeStatus == lifeStatus.GetEnumValueByMemberValue<LifeStatus>() &&
                    x.Gender == gender.GetEnumValueByMemberValue<Gender>()
                )
                .Skip(from)
                .Take(size)
                .OrderBy(x => x.Id)
                .ToListAsync())
                .Select(x => new AnimalResponseContract(x));
        }

        /// <summary>
        /// Добавление нового животного
        /// </summary>
        public async Task<AnimalResponseContract> AddAnimalAsync(AddingAnimalRequestContract contract)
        {
            await _animalEnsureService.EnsureAnimalTypesExistAsync(contract.AnimalTypes);
            await _animalEnsureService.EnsureChiperExistsAsync(contract.ChipperId);
            await _animalEnsureService.EnsureChippingLocationExistsAsync(contract.ChippingLocationId);

            _animalEnsureService.EnsureAnimalTypesNotRepeated(contract);
            var animalTypes = await _databaseContext.AnimalTypes
                .Where(x => contract.AnimalTypes.Contains(x.Id))
                .ToListAsync();

            var newAnimal = new Database.Models.Animals.Animal
            {
                AnimalTypes = animalTypes,
                Weight = contract.Weight.GetValueOrDefault(),
                Length = contract.Length.GetValueOrDefault(),
                Height = contract.Height.GetValueOrDefault(),
                Gender = contract.Gender.GetEnumValueByMemberValue<Gender>(),
                ChipperId = contract.ChipperId.GetValueOrDefault(),
                ChippingLocationPointId = contract.ChippingLocationId.GetValueOrDefault(),
            };

            await _databaseContext.AddAsync(newAnimal);
            await _databaseContext.SaveChangesAsync();

            return new AnimalResponseContract(newAnimal);
        }

        /// <summary>
        /// Обновление информации о животном 
        /// </summary>
        public async Task<AnimalResponseContract> UpdateAnimalAsync(
            long animalId,
            UpdatingAnimalRequestContract contract)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);

            animal.Weight = contract.Weight.GetValueOrDefault();
            animal.Length = contract.Length.GetValueOrDefault();
            animal.Height = contract.Height.GetValueOrDefault();
            animal.Gender = contract.Gender.GetEnumValueByMemberValue<Gender>();
            animal.ChipperId = contract.ChipperId.GetValueOrDefault();
            animal.ChippingLocationPointId = contract.ChippingLocationId.GetValueOrDefault();
            animal.LifeStatus = contract.LifeStatus.GetEnumValueByMemberValue<LifeStatus>();

            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Удаление животного
        /// </summary>
        public async Task DeleteAnimalAsync(long animalId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            _animalEnsureService.EnsureAnimalLiveChippingLocationButHasOther(animal);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        public async Task<AnimalResponseContract> AddAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            var type = await _animalTypeEnsureService.EnsureAnimalTypeExists(typeId);

            animal.AnimalTypes.Add(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        ///  Изменение типа животного у животного
        /// </summary>
        public async Task<AnimalResponseContract> UpdateAnimalTypeAsync(
            long animalId,
            TypeRequestContract contract)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            _ = await _animalTypeEnsureService.EnsureAnimalTypeExists(contract.OldTypeId.GetValueOrDefault());
            var type = _animalEnsureService.EnsureAnimalHasType(contract.OldTypeId.GetValueOrDefault(), animal);
            _animalEnsureService.EnsureAnimalHasNotType(contract.NewTypeId.GetValueOrDefault(), animal);
            var newType = await _animalTypeEnsureService.EnsureAnimalTypeExists(contract.NewTypeId.GetValueOrDefault());

            animal.AnimalTypes.Remove(type);
            animal.AnimalTypes.Add(newType);

            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Удаление типа животного у животного
        /// </summary>
        public async Task<AnimalResponseContract> DeleteAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExists(animalId);
            var type = _animalEnsureService.EnsureAnimalHasType(typeId, animal);
            _animalEnsureService.EnsureNotLastType(animal);
            animal.AnimalTypes.Remove(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }
    }
}