using DripChipDbSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Utils;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    /// <summary>
    /// Аттрибут валидации <see cref="AnimalRequestContract"/>
    /// </summary>
    public class AnimalRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AnimalRequestContract contract)
            {
                throw new InvalidCastException();
            }

            if (contract.AnimalTypes is null || !contract.AnimalTypes.Any())
            {
                throw new BadRequest400Exception();
            }

            if (contract.AnimalTypes.Any(x => x is null or <= 0))
            {
                throw new BadRequest400Exception();
            }

            if (contract.Weight is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            if (contract.Length is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            if (contract.Height is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            if (EnumUtils.GetValues<Gender>()
                .All(x => x.GetMemberValue() != contract.Gender))
            {
                throw new BadRequest400Exception();
            }

            if (contract.ChipperId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

#pragma warning disable IDE0046
            if (contract.ChippingLocationId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}
