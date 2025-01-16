using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace MultiAuthentication.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options, 
    ILoggerFactory logger, 
    UrlEncoder encoder) 
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues apiKeyHeaderValues))
        {
            // header cannot be found or parsed
            return Task.FromResult(AuthenticateResult.Fail("Api Key was not provided."));
        }

        string? providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        // should be move to application layer and handle logic in an ApplicationService
        if (string.Equals(providedApiKey, "yourApiKey", StringComparison.Ordinal))
        {
            // return authenticated user with Jwt, AuthTicket, ...
            Claim[] claims = { new(ClaimTypes.Name, "ApiKeyUser") };
            ClaimsIdentity identity = new(claims, "ApiKey");
            AuthenticationTicket ticket = new(new ClaimsPrincipal(identity), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Invalid API Key."));
    }
}