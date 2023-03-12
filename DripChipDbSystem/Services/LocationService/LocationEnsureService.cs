using System;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.LocationService
{
    /// <summary>
    /// Сервис проверок
    /// </summary>
    public class LocationEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public LocationEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Убедиться, что локации не существует
        /// </summary>
        public async Task EnsureLocationNotExistsAsync(LocationRequestContract contract)
        {
            const float epsilon = 0.000001f;
            var isExists = await _databaseContext.LocationPoints
                .AnyAsync(x => Math.Abs(x.Latitude - contract.Latitude.Value) < epsilon &&
                               Math.Abs(x.Longitude - contract.Longitude.Value) < epsilon);

            if (isExists)
            {
                throw new Conflict409Exception();
            }
        }

        /// <summary>
        /// Убедиться, что локация существует
        /// </summary>
        public async Task<LocationPoint> EnsureLocationExistsAsync(long pointId)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == pointId);

            return location ?? throw new NotFound404Exception();
        }

        /// <summary>
        /// Убедиться, что в локации никто не чипировался
        /// </summary>
        public async Task EnsureLocationNotChippingPointAsync(long pointId)
        {
            var isChippingPoint = await _databaseContext.Animals
                .AnyAsync(x => x.ChippingLocationPointId == pointId);

            if (isChippingPoint)
            {
                throw new BadRequest400Exception();
            }
        }

        /// <summary>
        /// Убедиться, что локацию никто не посещал
        /// </summary>
        public async Task EnsureLocationNotVisitedAsync(long pointId)
        {
            var isChippingPoint = await _databaseContext.Animals
                .AnyAsync(x => x.VisitedLocations
                    .Any(vl => vl.LocationPointId == pointId));

            if (isChippingPoint)
            {
                throw new BadRequest400Exception();
            }
        }
    }
}
