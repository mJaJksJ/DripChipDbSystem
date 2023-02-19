using System;
using DripChipDbSystem.Api.Controllers.AccountController;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    [LocationRequestValidation(typeof(LocationRequestContract))]
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

