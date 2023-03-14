using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Services.AccountService;
using DripChipDbSystem.Services.AnimalTypeService;
using DripChipDbSystem.Services.LocationService;
using DripChipDbSystem.Utils;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalService
{
    /// <summary>
    /// Сервис работы с животными
    /// </summary>
    public class AnimalService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly AnimalTypeEnsureService _animalTypeEnsureService;
        private readonly AnimalEnsureService _animalEnsureService;
        private readonly AccountEnsureService _accountEnsureService;
        private readonly LocationEnsureService _locationEnsureService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalService(
            DatabaseContext databaseContext,
            AnimalTypeEnsureService animalTypeEnsureService,
            AnimalEnsureService animalEnsureService,
            AccountEnsureService accountEnsureService,
            LocationEnsureService locationEnsureService)
        {
            _databaseContext = databaseContext;
            _animalTypeEnsureService = animalTypeEnsureService;
            _animalEnsureService = animalEnsureService;
            _accountEnsureService = accountEnsureService;
            _locationEnsureService = locationEnsureService;
        }

        /// <summary>
        /// Получение информации о животном
        /// </summary>
        public async Task<AnimalResponseContract> GetAnimalAsync(long animalId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);

            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Поиск животных по параметрам
        /// </summary>
        public async Task<IEnumerable<AnimalResponseContract>> SearchAsync(
            DateTimeOffset? startDateTime,
            DateTimeOffset? endDateTime,
            int? chipperId,
            long? chippingLocationId,
            string lifeStatus,
            string gender,
            int? from,
            int? size)
        {
            var animals = await _databaseContext.Animals
                .AsNoTracking()
                .Include(x => x.AnimalTypes)
                .Include(x => x.VisitedLocations)
                .Where(x =>
                    (startDateTime == null || x.ChippingDateTime >= startDateTime) &&
                    (endDateTime == null || x.ChippingDateTime <= endDateTime) &&
                    (chipperId == null || x.ChipperId == chipperId) &&
                    (chippingLocationId == null || x.ChippingLocationPointId == chippingLocationId) &&
                    (string.IsNullOrEmpty(lifeStatus) || x.LifeStatus == lifeStatus.GetEnumValueByMemberValue<LifeStatus>()) &&
                    (string.IsNullOrEmpty(gender) || x.Gender == gender.GetEnumValueByMemberValue<Gender>())
                )
                .OrderBy(x => x.Id)
                .Skip(from ?? 0)
                .Take(size ?? 10)
                .ToListAsync();
            return animals.Select(x => new AnimalResponseContract(x));
        }

        /// <summary>
        /// Добавление нового животного
        /// </summary>
        public async Task<AnimalResponseContract> AddAnimalAsync(AddingAnimalRequestContract contract)
        {
            await _animalTypeEnsureService.EnsureAnimalTypesExistAsync(contract.AnimalTypes);
            var chipper = await _accountEnsureService.EnsureAccountExistsAsync(contract.ChipperId.GetValueOrDefault());
            var chippingLocation = await _locationEnsureService.EnsureLocationExistsAsync(contract.ChippingLocationId.GetValueOrDefault());
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
                Chipper = chipper,
                ChippingLocationPoint = chippingLocation,
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
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            var chipper = await _accountEnsureService.EnsureAccountExistsAsync(contract.ChipperId.GetValueOrDefault());
            var location = await _locationEnsureService.EnsureLocationExistsAsync(contract.ChippingLocationId.GetValueOrDefault());
            _animalEnsureService.EnsureChangingNotFirstLocation(animal, location);

            animal.Weight = contract.Weight.GetValueOrDefault();
            animal.Length = contract.Length.GetValueOrDefault();
            animal.Height = contract.Height.GetValueOrDefault();
            animal.Gender = contract.Gender.GetEnumValueByMemberValue<Gender>();
            animal.Chipper = chipper;
            animal.ChippingLocationPoint = location;
            animal.LifeStatus = contract.LifeStatus.GetEnumValueByMemberValue<LifeStatus>();

            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Удаление животного
        /// </summary>
        public async Task DeleteAnimalAsync(long animalId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            _animalEnsureService.EnsureAnimalLeftChippingLocationButHasOther(animal);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        public async Task<AnimalResponseContract> AddAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            var type = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(typeId);

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
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            _ = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(contract.OldTypeId.GetValueOrDefault());
            var type = _animalEnsureService.EnsureAnimalHasType(contract.OldTypeId.GetValueOrDefault(), animal);
            _animalEnsureService.EnsureAnimalHasNotType(contract.NewTypeId.GetValueOrDefault(), animal);
            var newType = await _animalTypeEnsureService.EnsureAnimalTypeExistsAsync(contract.NewTypeId.GetValueOrDefault());

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
            var animal = await _animalEnsureService.EnsureAnimalExistsAsync(animalId);
            var type = _animalEnsureService.EnsureAnimalHasType(typeId, animal);
            _animalEnsureService.EnsureNotLastType(animal);

            animal.AnimalTypes.Remove(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }
    }
}
