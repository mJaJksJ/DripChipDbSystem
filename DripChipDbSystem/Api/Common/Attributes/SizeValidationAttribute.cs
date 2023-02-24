using DripChipDbSystem.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace DripChipDbSystem.Api.Common.Attributes
{
    /// <summary>
    /// Аттрибут валидации параметра Size
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SizeValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            return value switch
            {
                > 0 => true,
                _ => throw new BadRequest400Exception(),
            };
        }
    }
}
