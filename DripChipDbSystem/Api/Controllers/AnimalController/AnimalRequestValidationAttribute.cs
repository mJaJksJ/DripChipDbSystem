using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using DripChipDbSystem.Database.Enums;
using DripChipDbSystem.Utils;

namespace DripChipDbSystem.Api.Controllers.AnimalController
{
    public class AnimalRequestValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public AnimalRequestValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            if (value is not AnimalRequestContract contract)
            {
                throw new Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.AnimalTypes is null || !contract.AnimalTypes.Any())
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.AnimalTypes.Any(x => x is null or <= 0))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.Weight is null or <= 0)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.Length is null or <= 0)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.Height is null or <= 0)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (EnumUtils.GetValues<Gender>().All(x => x.GetMemberValue() != contract.Gender))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.ChipperId is null or <= 0)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (contract.ChippingLocationId is null or <= 0)
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
