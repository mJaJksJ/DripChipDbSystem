namespace DripChipDbSystem.Api.Controllers.LocationPointController
{
    /// <summary>
    /// Информации о точке локации животных
    /// </summary>
    public class LocationPointInfoResponse
    {
        /// <summary>
        /// Идентификатор точки локации
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Географическая широта в градусах
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///  Географическая долгота в градусах
        /// </summary>
        public double Longitude { get; set; }
    }
}
