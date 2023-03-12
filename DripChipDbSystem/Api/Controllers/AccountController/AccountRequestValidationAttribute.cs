using System;
using System.ComponentModel.DataAnnotations;
using DripChipDbSystem.Api.Common;
using DripChipDbSystem.Exceptions;

namespace DripChipDbSystem.Api.Controllers.AccountController
{
    /// <summary>
    /// Аттрибут валидации <see cref="AccountRequestContract"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AccountRequestValidationAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public override bool IsValid(object value)
        {
            if (value is not AccountRequestContract contract)
            {
                throw new InvalidCastException();
            }

            if (string.IsNullOrEmpty(contract.FirstName) ||
                Regexes.OnlySpaceSymbols.IsMatch(contract.FirstName))
            {
                throw new BadRequest400Exception();
            }

            if (string.IsNullOrEmpty(contract.LastName) ||
                Regexes.OnlySpaceSymbols.IsMatch(contract.LastName))
            {
                throw new BadRequest400Exception();
            }

            if (string.IsNullOrEmpty(contract.Email) ||
                Regexes.OnlySpaceSymbols.IsMatch(contract.Email) ||
                !Regexes.Email.IsMatch(contract.Email))
            {
                throw new BadRequest400Exception();
            }

#pragma warning disable IDE0046
            if (string.IsNullOrEmpty(contract.Password) ||
                string.IsNullOrEmpty(contract.Password) ||
                Regexes.OnlySpaceSymbols.IsMatch(contract.Password))
            {
                throw new BadRequest400Exception();
            }

            return true;
        }
    }
}
