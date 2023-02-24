namespace DripChipDbSystem.Api.Controllers.LocationController
{
    /// <summary>
    /// Контракт запроса точки локации
    /// </summary>
    [LocationRequestValidation]
    public class LocationRequestContract
    {
        /// <summary>
        /// Географическая широта в градусах
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        ///  Географическая долгота в градусах
        /// </summary>
        public double? Longitude { get; set; }
    }
}

