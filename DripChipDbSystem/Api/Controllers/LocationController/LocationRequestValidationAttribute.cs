using System;
using System.ComponentModel.DataAnnotations;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;

namespace DripChipDbSystem.Api.Controllers.LocationController
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LocationRequestValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public LocationRequestValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            if (value is not LocationRequestContract contract)
            {
                throw new Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.Latitude is null || contract.Latitude < -90 || contract.Latitude > 90)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.Longitude is null || contract.Longitude < -90 || contract.Longitude > 90)
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

