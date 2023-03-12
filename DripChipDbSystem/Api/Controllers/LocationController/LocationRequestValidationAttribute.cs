using System;
using System.ComponentModel.DataAnnotations;
using DripChipDbSystem.Exceptions;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    /// <summary>
    /// Аттрибут валидации <see cref="LocationRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LocationRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not LocationRequestContract contract)
            {
                throw new InvalidCastException();
            }

            if (contract.Latitude is null or < -90 or > 90)
            {
                throw new BadRequest400Exception();
            }

#pragma warning disable IDE0046
            if (contract.Longitude is null or < -180 or > 180)
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}

