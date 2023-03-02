using DripChipDbSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System;
using DripChipDbSystem.Api.Common;

namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    /// <summary>
    /// Аттрибут валидации <see cref="AnimalTypeRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AnimalTypeRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AnimalTypeRequestContract contract)
            {
                throw new InvalidCastException();
            }

#pragma warning disable IDE0046
            if (string.IsNullOrEmpty(contract.Type) ||
                Regexes.OnlySpaceSymbols.IsMatch(contract.Type))
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}
