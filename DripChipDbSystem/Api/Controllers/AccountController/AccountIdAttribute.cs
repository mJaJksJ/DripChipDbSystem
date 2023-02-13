using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using System.ComponentModel.DataAnnotations;
using System;
using DripChipDbSystem.Exceptions;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class AccountIdAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public AccountIdAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        public override bool IsValid(object value)
        {
            if (value is not int accountId)
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (accountId <= 0)
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
