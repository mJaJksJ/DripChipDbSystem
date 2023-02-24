using DripChipDbSystem.Database.Models.Animals;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    /// <summary>
    /// Ответ на запрос точки локации
    /// </summary>
    public class LocationResponseContract
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

        /// <summary>
        /// .ctor
        /// </summary>
        public LocationResponseContract(LocationPoint location)
        {
            Id = location.Id;
            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }
    }
}

