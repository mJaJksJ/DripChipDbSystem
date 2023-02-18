using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DripChipDbSystem.Api.Controllers.AccountController;
using DripChipDbSystem.Database;
using DripChipDbSystem.Database.Models.Auth;
using DripChipDbSystem.Exceptions;
using DripChipDbSystem.Middlewares.HttpResponseMiddleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DripChipDbSystem.Services
{
    public class AccountService
    {
        private readonly DatabaseContext _databaseContext;

        public AccountService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<AccountResponseContract> GetAccountAsync(int accountId)
        {
            var account = await _databaseContext.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == accountId);

            if (account is null)
            {
                throw new NotFound404Exception() { Data = { { HttpResponseMiddleware.ResultKey, new AccountResponseContract() } } };
            }

            return new AccountResponseContract
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email
            };
        }

        public async Task<IEnumerable<AccountResponseContract>> SearchAsync(string firstName, string lastName, string email, int from, int size)
        {
            return await _databaseContext.Accounts
                .Where(Filter(x => x.FirstName, firstName))
                .Where(Filter(x => x.LastName, lastName))
                .Where(Filter(x => x.Email, email))
                .Skip(from)
                .Take(size)
                .Select(x => new AccountResponseContract
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                })
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        private static Expression<Func<Account, bool>> Filter(Func<Account, string> field, string filterString)
        {
            return account => filterString != null && field(account).ToLower().Contains(filterString.ToLower());
        }
    }
}
