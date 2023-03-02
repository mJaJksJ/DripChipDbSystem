using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AnimalTypeController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.AnimalType
{
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
        public async Task EnsureAnimalTypeNotExists(AnimalTypeRequestContract contract)
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
        public async Task<Database.Models.Animals.AnimalType> EnsureAnimalTypeExists(long id)
        {
            var animalType = await _databaseContext.AnimalTypes
                .SingleOrDefaultAsync(x => x.Id == id);

            return animalType ?? throw new NotFound404Exception();
        }

        public async Task EnsureTypeHasNoAnimals(long typeId)
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
