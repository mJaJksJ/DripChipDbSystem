using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using DripChipDbSystem.Exceptions;
using System;
using System.Linq;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Attributes
{
    /// <summary>
    /// <inheritdoc cref="AnimalRequestValidationAttribute"/>.
    /// <see cref="AddingAnimalRequestContract"/>
    /// </summary>
    public class AddingAnimalRequestValidationAttribute : AnimalRequestValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AddingAnimalRequestContract contract)
            {
                throw new InvalidCastException();
            }

#pragma warning disable IDE0046
            if (contract.AnimalTypes is null || !contract.AnimalTypes.Any())
            {
                throw new BadRequest400Exception();
            }

            if (contract.AnimalTypes.Any(x => x is null or <= 0))
            {
                throw new BadRequest400Exception();
            }

            return base.IsValid(value);
        }
    }
}
