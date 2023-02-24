using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    [AttributeUsage(AttributeTargets.Class)]
    public partial class AccountRequestValidationAttribute : ValidationAttribute
    {
        private readonly object _defaultValue;

        public AccountRequestValidationAttribute(Type type)
        {
            _defaultValue = Activator.CreateInstance(type);
        }

        [GeneratedRegex(@"^\s*$")]
        private static partial Regex OnlySpaceSymbols();

        [GeneratedRegex(@"^[A-Z0-9._%+-]+@([A-Z0-9-]+.+.[A-Z]{2,4})?$")]
        private static partial Regex Email();

        public override bool IsValid(object value)
        {
            if (value is not AccountRequestContract contract)
            {
                throw new Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (string.IsNullOrEmpty(contract.FirstName) ||
                OnlySpaceSymbols().IsMatch(contract.FirstName))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (string.IsNullOrEmpty(contract.LastName) ||
                OnlySpaceSymbols().IsMatch(contract.LastName))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (OnlySpaceSymbols().IsMatch(contract.Email) || Email().IsMatch(contract.Email))
            {
                throw new BadRequest400Exception()
                {
                    Data = { { HttpResponseMiddleware.ResultKey, _defaultValue } }
                };
            }

            if (string.IsNullOrEmpty(contract.Password) ||
                OnlySpaceSymbols().IsMatch(contract.Password))
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
