using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.ComponentModel.DataAnnotations;
using System;

namespace DripChipDbSystem.Api.Controllers.AnimalTypeController
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnimalTypeValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public AnimalTypeValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            if (value is not AnimalTypeRequestContract contract)
            {
                throw new Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (string.IsNullOrEmpty(contract.Type))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            return true;
        }
    }
}
