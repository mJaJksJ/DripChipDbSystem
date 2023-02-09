using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DripChipDbSystem.Api.Controllers.AuthControllers
{
    [AttributeUsage(AttributeTargets.Class)]
    public partial class AuthRequestValidationAttribute : ValidationAttribute
    {
        [GeneratedRegex(@"^\s*$")]
        private static partial Regex OnlySpaceSymbols();

        [GeneratedRegex(@"^\s*$")]
        private static partial Regex Email();

        public override bool IsValid(object value)
        {
            if (value is not AuthRequestContract contract)
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(contract.FirstName) ||
                OnlySpaceSymbols().IsMatch(contract.FirstName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(contract.LastName) ||
                OnlySpaceSymbols().IsMatch(contract.LastName))
            {
                return false;
            }

            if (OnlySpaceSymbols().IsMatch(contract.Email) || Email().IsMatch(contract.Email))
            {
                return false;
            }

            if (string.IsNullOrEmpty(contract.Password) ||
                OnlySpaceSymbols().IsMatch(contract.Password))
            {
                return false;
            }

            return true;
        }
    }
}
