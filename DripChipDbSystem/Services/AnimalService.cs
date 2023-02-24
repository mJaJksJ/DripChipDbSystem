using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Utils;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    /// <summary>
    /// Сервис работы с животными
    /// </summary>
    public class AnimalService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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
        public async Task<AnimalResponseContract> AddAnimalAsync(AnimalRequestContract contract)
        {
            var animalTypes = await _databaseContext.AnimalTypes
                .Where(x => contract.AnimalTypes.Contains(x.Id))
                .ToListAsync();

            var newAnimal = new Animal
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
            AnimalRequestContract contract)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);

            var animalTypes = await _databaseContext.AnimalTypes
                .Where(x => contract.AnimalTypes.Contains(x.Id))
                .ToListAsync();

            animal.AnimalTypes = animalTypes;
            animal.Weight = contract.Weight.GetValueOrDefault();
            animal.Length = contract.Length.GetValueOrDefault();
            animal.Height = contract.Height.GetValueOrDefault();
            animal.Gender = contract.Gender.GetEnumValueByMemberValue<Gender>();
            animal.ChipperId = contract.ChipperId.GetValueOrDefault();
            animal.ChippingLocationPointId = contract.ChippingLocationId.GetValueOrDefault();

            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Удаление животного
        /// </summary>
        public async Task DeleteAnimalAsync(long animalId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Добавление типа животного к животному
        /// </summary>
        public async Task<AnimalResponseContract> AddAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            var type = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

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
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);

            var type = animal.AnimalTypes.Single(x => x.Id == contract.OldTypeId);
            type.Id = contract.NewTypeId;
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }

        /// <summary>
        /// Удаление типа животного у животного
        /// </summary>
        public async Task<AnimalResponseContract> DeleteAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            var type = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

            animal.AnimalTypes.Remove(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract(animal);
        }
    }
}
