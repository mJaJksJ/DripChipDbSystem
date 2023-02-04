using System.Linq;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.LocationPointController;
using DripChipDbSystem.Database;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class LocationPointService
    {
        private readonly DatabaseContext _databaseContext;

        public LocationPointService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<LocationPointInfoResponse> GetLocationPointInfoAsync(long pointId)
        {
            return await _databaseContext.LocationPoints
                .Where(x => x.Id == pointId)
                .Select(x => new LocationPointInfoResponse
                {
                    Id = x.Id,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                })
                .SingleOrDefaultAsync();
        }
    }
}
