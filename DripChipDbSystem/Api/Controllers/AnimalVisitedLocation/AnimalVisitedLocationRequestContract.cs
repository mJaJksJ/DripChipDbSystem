using DripChipDbSystem.Api.Common.Attributes;
using DripChipDbSystem.Api.Controllers.AnimalController;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    public class AnimalVisitedLocationRequestContract
    {
        /// <summary>
        /// Идентификатор объекта с информацией о посещенной точке локации
        /// </summary>
        [IdValidation(typeof(AnimalResponseContract))]
        public long VisitedLocationPointId { get; set; }

        /// <summary>
        /// Идентификатор посещенной точки локации
        /// </summary>
        [IdValidation(typeof(AnimalResponseContract))]
        public long LocationPointId { get; set; }
    }
}
