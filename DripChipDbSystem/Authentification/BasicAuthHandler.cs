using System.Security.Claims;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DripChipDbSystem.Database;
using DripChipDbSystem.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace DripChipDbSystem.Authentification
{
    public partial class BasicAuthHandler : AuthenticationHandler<BasicAuthSchemeOptions>
    {
        private readonly DatabaseContext _databaseContext;

        [GeneratedRegex("^Basic [A-Z0-9._%+-]+@[A-Z0-9-]+.+.[A-Z]{2,4}:(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$")]
        private static partial Regex AuthHeaderRegex();

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

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail(""));
            }

            var header = Request.Headers[HeaderNames.Authorization].ToString();
            var tokenMatch = AuthHeaderRegex().Match(header);

            if (!tokenMatch.Success)
            {
                throw new Unauthorized401Exception();
            }

            var authData = header.Replace("Basic ", "").Split(':');
            var login = authData[0];
            var password = authData[1];
            var user = _databaseContext.Accounts
                .SingleOrDefault(x => x.Email == login && x.PasswordHash == password);

            if (user is null)
            {
                throw new Unauthorized401Exception();
            }

            var claimsIdentity = new ClaimsIdentity(nameof(BasicAuthHandler));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
