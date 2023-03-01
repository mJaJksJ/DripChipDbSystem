using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DripChipDbSystem.Authentification
{
    /// <summary>
    /// Обработчик простой аутентификации
    /// </summary>
    public class BasicAuthHandler : AuthenticationHandler<BasicAuthSchemeOptions>
    {
        private readonly DatabaseContext _databaseContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public BasicAuthHandler(
            IOptionsMonitor<BasicAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DatabaseContext databaseContext)
            : base(options, logger, encoder, clock)
        {
            _databaseContext = databaseContext;
        }

        /// <inheritdoc/>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail(""));
            }

            string[] authData;
            try
            {
                var header = Request.Headers[HeaderNames.Authorization].ToString();
                var base64EncodedBytes = Convert.FromBase64String(header.Replace("Basic ", ""));
                authData = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Split(':');
            }
            catch (FormatException)
            {
                throw new Unauthorized401Exception();
            }

            var login = authData[0];
            var password = authData[1];

            var user = _databaseContext.Accounts
                .SingleOrDefault(x => x.Email == login && x.PasswordHash == password);
            if (user is null)
            {
                throw new Unauthorized401Exception();
            }

            var claims = new List<Claim>
            {
                new(BasicAuth.Sid, user.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, nameof(BasicAuthHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
