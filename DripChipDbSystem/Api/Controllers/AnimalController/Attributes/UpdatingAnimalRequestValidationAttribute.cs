using DripChipDbSystem.Api.Controllers.AnimalController.Contracts;
using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Utils;

namespace DripChipDbSystem.Api.Controllers.AnimalController.Attributes
{
    /// <summary>
    /// <inheritdoc cref="AnimalRequestValidationAttribute"/>.
    /// <see cref="UpdatingAnimalRequestContract"/>
    /// </summary>
    public class UpdatingingAnimalRequestValidation : AnimalRequestValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not UpdatingAnimalRequestContract contract)
            {
                throw new InvalidCastException();
            }

#pragma warning disable IDE0046
            if (EnumUtils.GetValues<LifeStatus>()
                .All(x => x.GetMemberValue() != contract.LifeStatus))
            {
                throw new BadRequest400Exception();
            }

            return base.IsValid(value);
        }
    }
}
