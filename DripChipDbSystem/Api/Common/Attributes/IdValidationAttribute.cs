using System;
using System.ComponentModel.DataAnnotations;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;

namespace DripChipDbSystem.Api.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class IdValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public IdValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            return value switch
            {
                > 0 => true,
                long and > 0 => true,
                _ => throw new BadRequest400Exception() { Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } } }
            };
        }
    }
}
