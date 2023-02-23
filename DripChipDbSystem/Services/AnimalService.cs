using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Api.Controllers.AnimalController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using DripChipDbSystem.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class AnimalService
    {
        private readonly DatabaseContext _databaseContext;

        public AnimalService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<AnimalResponseContract> GetAnimalAsync(long animalId)
        {
            var animal = await _databaseContext.Animals
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == animalId);

            if (animal is null)
            {
                throw new NotFound404Exception() { Data = { { HttpResponseMiddleware.ResultKey, new AnimalResponseContract() } } };
            }

            return new AnimalResponseContract
            {
                Id = animal.Id,
                AnimalTypes = animal.AnimalTypes.Select(x => x.Id),
                Weight = animal.Weight,
                Length = animal.Length,
                Height = animal.Height,
                Gender = animal.Gender.GetMemberValue(),
                LifeStatus = animal.LifeStatus.GetMemberValue(),
                ChippingDateTime = animal.ChippingDateTime,
                ChipperId = animal.ChipperId,
                ChippingLocationPointId = animal.ChippingLocationPointId,
                VisitedLocations = animal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = animal.DeathDateTime.GetValueOrDefault(),
            };
        }

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
            return await _databaseContext.Animals
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
                .Select(x => new AnimalResponseContract
                {
                    Id = x.Id,
                    AnimalTypes = x.AnimalTypes.Select(x => x.Id),
                    Weight = x.Weight,
                    Length = x.Length,
                    Height = x.Height,
                    Gender = x.Gender.GetMemberValue(),
                    LifeStatus = x.LifeStatus.GetMemberValue(),
                    ChippingDateTime = x.ChippingDateTime,
                    ChipperId = x.ChipperId,
                    ChippingLocationPointId = x.ChippingLocationPointId,
                    VisitedLocations = x.VisitedLocations.Select(l => l.Id),
                    DeathDateTime = x.DeathDateTime.GetValueOrDefault()
                })
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<AnimalResponseContract> AddAnimalAsync(AnimalRequestContract contract)
        {
            var animalTypes = await _databaseContext.AnimalTypes
                .Where(x => contract.AnimalTypes.Contains(x.Id))
                .ToListAsync();

            var newAnimal = new Animal
            {
                AnimalTypes = animalTypes,
                Weight = contract.Weight.Value,
                Length = contract.Length.Value,
                Height = contract.Height.Value,
                Gender = contract.Gender.GetEnumValueByMemberValue<Gender>(),
                ChipperId = contract.ChipperId.Value,
                ChippingLocationPointId = contract.ChippingLocationId.Value,
            };

            await _databaseContext.AddAsync(newAnimal);
            await _databaseContext.SaveChangesAsync();

            return new AnimalResponseContract
            {
                Id = newAnimal.Id,
                AnimalTypes = newAnimal.AnimalTypes.Select(x => x.Id),
                Weight = newAnimal.Weight,
                Length = newAnimal.Length,
                Height = newAnimal.Height,
                Gender = newAnimal.Gender.GetMemberValue(),
                LifeStatus = newAnimal.LifeStatus.GetMemberValue(),
                ChippingDateTime = newAnimal.ChippingDateTime,
                ChipperId = newAnimal.ChipperId,
                ChippingLocationPointId = newAnimal.ChippingLocationPointId,
                VisitedLocations = newAnimal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = newAnimal.DeathDateTime.GetValueOrDefault(),
            };
        }

        public async Task<AnimalResponseContract> UpdateAnimalAsync(long animalId,
            AnimalRequestContract contract)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);

            var animalTypes = await _databaseContext.AnimalTypes
                .Where(x => contract.AnimalTypes.Contains(x.Id))
                .ToListAsync();

            animal.AnimalTypes = animalTypes;
            animal.Weight = contract.Weight.Value;
            animal.Length = contract.Length.Value;
            animal.Height = contract.Height.Value;
            animal.Gender = contract.Gender.GetEnumValueByMemberValue<Gender>();
            animal.ChipperId = contract.ChipperId.Value;
            animal.ChippingLocationPointId = contract.ChippingLocationId.Value;

            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract
            {
                Id = animal.Id,
                AnimalTypes = animal.AnimalTypes.Select(x => x.Id),
                Weight = animal.Weight,
                Length = animal.Length,
                Height = animal.Height,
                Gender = animal.Gender.GetMemberValue(),
                LifeStatus = animal.LifeStatus.GetMemberValue(),
                ChippingDateTime = animal.ChippingDateTime,
                ChipperId = animal.ChipperId,
                ChippingLocationPointId = animal.ChippingLocationPointId,
                VisitedLocations = animal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = animal.DeathDateTime.GetValueOrDefault(),
            };
        }

        public async Task DeleteAnimalAsync(long animalId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            _databaseContext.Remove(animal);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<AnimalResponseContract> AddAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            var type = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

            animal.AnimalTypes.Add(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract
            {
                Id = animal.Id,
                AnimalTypes = animal.AnimalTypes.Select(x => x.Id),
                Weight = animal.Weight,
                Length = animal.Length,
                Height = animal.Height,
                Gender = animal.Gender.GetMemberValue(),
                LifeStatus = animal.LifeStatus.GetMemberValue(),
                ChippingDateTime = animal.ChippingDateTime,
                ChipperId = animal.ChipperId,
                ChippingLocationPointId = animal.ChippingLocationPointId,
                VisitedLocations = animal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = animal.DeathDateTime.GetValueOrDefault(),
            };
        }

        public async Task<AnimalResponseContract> UpdateAnimalTypeAsync(long animalId, TypeRequestContract contract)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);

            var type = animal.AnimalTypes.Single(x => x.Id == contract.OldTypeId);
            type.Id = contract.NewTypeId;
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract
            {
                Id = animal.Id,
                AnimalTypes = animal.AnimalTypes.Select(x => x.Id),
                Weight = animal.Weight,
                Length = animal.Length,
                Height = animal.Height,
                Gender = animal.Gender.GetMemberValue(),
                LifeStatus = animal.LifeStatus.GetMemberValue(),
                ChippingDateTime = animal.ChippingDateTime,
                ChipperId = animal.ChipperId,
                ChippingLocationPointId = animal.ChippingLocationPointId,
                VisitedLocations = animal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = animal.DeathDateTime.GetValueOrDefault(),
            };
        }

        public async Task<AnimalResponseContract> DeleteAnimalTypeAsync(long animalId, long typeId)
        {
            var animal = await _databaseContext.Animals
                .SingleOrDefaultAsync(x => x.Id == animalId);
            var type = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == typeId);

            animal.AnimalTypes.Remove(type);
            await _databaseContext.SaveChangesAsync();
            return new AnimalResponseContract
            {
                Id = animal.Id,
                AnimalTypes = animal.AnimalTypes.Select(x => x.Id),
                Weight = animal.Weight,
                Length = animal.Length,
                Height = animal.Height,
                Gender = animal.Gender.GetMemberValue(),
                LifeStatus = animal.LifeStatus.GetMemberValue(),
                ChippingDateTime = animal.ChippingDateTime,
                ChipperId = animal.ChipperId,
                ChippingLocationPointId = animal.ChippingLocationPointId,
                VisitedLocations = animal.VisitedLocations.Select(x => x.Id),
                DeathDateTime = animal.DeathDateTime.GetValueOrDefault(),
            };
        }
    }
}
