namespace DripChipDbSystem.Database.Models.Animals
{
    /// <summary>
    /// Точка локации
    /// </summary>
    public class LocationPoint
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Географическая широта в градусах
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Географическая долгота в градусах
        /// </summary>
        public double Longitude { get; set; }
    }
}
