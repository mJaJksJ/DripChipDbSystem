using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalTypeController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalTypeService
{
    /// <summary>
    /// Сервис проверок
    /// </summary>
    public class AnimalTypeEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AnimalTypeEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Убедиться, что типа животного не существует
        /// </summary>
        public async Task EnsureAnimalTypeNotExistsAsync(AnimalTypeRequestContract contract)
        {
            var isExists = await _databaseContext.AnimalTypes
                .AnyAsync(x => x.Type == contract.Type);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        /// <summary>
        /// Убедиться, что тип животного существует
        /// </summary>
        public async Task<AnimalType> EnsureAnimalTypeExistsAsync(long id)
        {
            var animalType = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == id);

            return animalType ?? throw new NotFound404Exception();
        }


        /// <summary>
        /// Убедиться, что типы животного существуют
        /// </summary>
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

        /// <summary>
        /// Убедиться, что тип не связан с животными
        /// </summary>
        public async Task EnsureTypeHasNoAnimalsAsync(long typeId)
        {
            var hasAnimals = await _databaseContext.Animals
                .AnyAsync(x => x.AnimalTypes
                    .Any(at => at.Id == typeId));

            if (hasAnimals)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
