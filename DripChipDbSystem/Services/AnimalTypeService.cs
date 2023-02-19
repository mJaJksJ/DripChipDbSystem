using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalTypeController;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class AnimalTypeService
    {
        private readonly DatabaseContext _databaseContext;

        public AnimalTypeService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<AnimalTypeResponseContract> GetAnimalTypeAsync(long typeId)
        {
            var animalType = await _databaseContext.AnimalTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == typeId);

            if (animalType is null)
            {
                throw new NotFound404Exception() { Data = { { HttpResponseMiddleware.ResultKey, new AnimalTypeResponseContract() } } };
            }

            return new AnimalTypeResponseContract()
            {
                Id = animalType.Id,
                Type = animalType.Type
            };
        }

        public async Task<AnimalTypeResponseContract> AddAnimalTypeAsync(AnimalTypeRequestContract contract)
        {
            var animalType = await _databaseContext.AnimalTypes
                .AddAsync(new AnimalType
                {
                    Type = contract.Type,
                });
            await _databaseContext.SaveChangesAsync();
            return new AnimalTypeResponseContract
            {
                Id = animalType.Entity.Id,
                Type = animalType.Entity.Type,
            };
        }

        public async Task<AnimalTypeResponseContract> UpdateAnimalTypeAsync(long pointId, AnimalTypeRequestContract contract)
        {
            var location = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == pointId);

            location.Type = contract.Type;

            await _databaseContext.SaveChangesAsync();
            return new AnimalTypeResponseContract
            {
                Id = location.Id,
                Type = location.Type,
            };
        }

        public async Task DeleteAnimalTypeAsync(long pointId)
        {
            var location = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == pointId);

            _databaseContext.Remove(location);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task EnsureAnimalTypeNotExists(AnimalTypeRequestContract contract)
        {
            var isExists = await _databaseContext.AnimalTypes
                .AnyAsync(x => x.Type == contract.Type);

            if (isExists)
            {
                throw new Conflict409Exception() { Data = { { HttpResponseMiddleware.ResultKey, new AccountResponseContract() } } };
            }
        }
    }
}
