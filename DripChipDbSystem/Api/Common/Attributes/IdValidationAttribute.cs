using System;
using System.ComponentModel.DataAnnotations;
using DripChipDbSystem.Exceptions;

namespace DripChipDbSystem.Api.Common.Attributes
{
    /// <summary>
    /// Аттрибут валидации параметра From
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class IdValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            return value switch
            {
                > 0 => true,
                long and > 0 => true,
                _ => throw new BadRequest400Exception(),
            };
        }
    }
}
