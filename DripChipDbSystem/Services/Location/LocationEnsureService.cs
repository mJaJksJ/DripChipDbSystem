using System;
using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.LocationController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Animals;
using DripChipDbSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services.Location
{
    public class LocationEnsureService
    {
        private readonly DatabaseContext _databaseContext;

        public LocationEnsureService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task EnsureLocationNotExists(LocationRequestContract contract)
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

        public async Task<LocationPoint> EnsureLocationExists(long pointId)
        {
            var location = await _databaseContext.LocationPoints
                .SingleOrDefaultAsync(x => x.Id == pointId);

            return location ?? throw new NotFound404Exception();
        }

        public async Task EnsureLocationNotChippingPoint(long pointId)
        {
            var isChippingPoint = await _databaseContext.Animals
                .AnyAsync(x => x.ChippingLocationPointId == pointId);

            if (isChippingPoint)
            {
                throw new BadRequest400Exception();
            }
        }
        public async Task EnsureLocationNotVisited(long pointId)
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
