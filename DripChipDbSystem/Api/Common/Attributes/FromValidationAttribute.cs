using DripChipDbSystem.Exceptions;
using System.ComponentModel.DataAnnotations;
using System;

namespace DripChipDbSystem.Api.Common.Attributes
{
    /// <summary>
    /// Аттрибут валидации параметра From
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            return value switch
            {
                >= 0 => true,
                _ => throw new BadRequest400Exception(),
            };
        }
    }
}
