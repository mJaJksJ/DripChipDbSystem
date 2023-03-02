using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using DripChipDbSystem.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Attributes
{
    /// <summary>
    /// Аттрибут валидации <see cref="TypeRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TypeRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not TypeRequestContract contract)
            {
                throw new InvalidCastException();
            }

#pragma warning disable IDE0046
            if (contract.OldTypeId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            if (contract.NewTypeId is null or <= 0)
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}
