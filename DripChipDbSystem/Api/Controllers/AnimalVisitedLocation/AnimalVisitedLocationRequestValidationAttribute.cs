using DripChipDbSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System;

namespace DripChipDbSystem.Api.Controllers.AnimalVisitedLocation
{
    /// <summary>
    /// Аттрибут валидации <see cref="AnimalVisitedLocationRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AnimalVisitedLocationRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AnimalVisitedLocationRequestContract contract)
            {
                throw new InvalidCastException();
            }

#pragma warning disable IDE0046
            if (contract.VisitedLocationPointId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            if (contract.LocationPointId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}
