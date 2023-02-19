using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.ComponentModel.DataAnnotations;
using System;

namespace DripChipDbSystem.Api.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public FromValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            return value switch
            {
                >= 0 => true,
                _ => throw new BadRequest400Exception() { Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } } }
            };
        }
    }
}
