using System.Threading.Tasks;
using DripChipDbSystem.Services;

namespace DripChipDbSystem.Api.Controllers.LocationPointController
{
    public class LocationPointController
    {
        private readonly LocationPointService _locationPointService;

        public LocationPointController(LocationPointService locationPointService)
        {
            _locationPointService = locationPointService;
        }

        public async Task<LocationPointInfoResponse> GetAsync(long pointId)
        {
            return await _locationPointService.GetLocationPointInfoAsync(pointId);
        }
    }
}
