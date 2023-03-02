using DripChipDbSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Utils;
using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Attributes
{
    /// <summary>
    /// Аттрибут валидации <see cref="AnimalRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AnimalRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AnimalRequestContract contract)
            {
                throw new InvalidCastException();
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
