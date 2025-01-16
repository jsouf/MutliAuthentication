using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiAuthentication.Authentication;

namespace MultiAuthentication;

public static class DependancyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services)
    {
        return services
            .AddJwtAuthentication()
            .AddApiKeyAuthentication()
            .AddApiAuthorization();
    }

    private static IServiceCollection AddApiAuthorization(this IServiceCollection services)
    {
        return services
            .AddAuthorization(options =>
            {
                // define common policy for all authentications
                options.AddPolicy(AuthenticationConstantes.MultiAuthenticationPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, AuthenticationConstantes.ApiKeyAuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = AuthenticationConstantes.SharedScheme;
                options.DefaultChallengeScheme = AuthenticationConstantes.SharedScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "issuer", // change by token issuer provider
                    ValidAudience = "audience", // change by authority audience provider
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstantes.SecretKey))
                };
            });

        return services;
    }

    private static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(AuthenticationConstantes.ApiKeyAuthenticationScheme, null);

        return services;
    }
}
