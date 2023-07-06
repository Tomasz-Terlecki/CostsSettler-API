using CostsSettler.Auth.Clients;
using CostsSettler.Auth.Config;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using CostsSettler.Repo.Repositories;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.IdentityModel.Tokens;

namespace CostsSettler.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICircumstanceRepository, CircumstanceRepository>();
        services.AddScoped<IChargeRepository, ChargeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }

    public static IServiceCollection AddKeycloakClient(this IServiceCollection services, KeycloakClientConfig config, bool ignoreSSL = false)
    {
        var keycloakClient = new KeycloakClient(config, ignoreSSL);

        services.AddScoped<IKeycloakClient>((serviceProvider) => keycloakClient);

        return services;
    }

    public static IServiceCollection AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration config, IHostEnvironment env)
    {
        var keycloakClientConfig = config.GetSection("KeycloakClientConfig").Get<KeycloakClientConfig>();
        services.AddKeycloakClient(keycloakClientConfig, env.IsDevelopment());

        var authenticationOptions = new KeycloakAuthenticationOptions
        {
            AuthServerUrl = keycloakClientConfig.AuthServerUrl,
            Realm = keycloakClientConfig.Realm,
            Resource = keycloakClientConfig.ClientId,
            VerifyTokenAudience = false,
            Credentials = new KeycloakClientInstallationCredentials
            {
                Secret = keycloakClientConfig.Secret,
            }
        };

        services.AddKeycloakAuthentication(authenticationOptions, opts =>
        {
            opts.RequireHttpsMetadata = false;

            // Prevent middleware to modify claims after successful authentication,
            // enables using "roles" and "name" claims in authorization attributes
            opts.MapInboundClaims = false;
            opts.SaveToken = env.IsDevelopment();

            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidateAudience = false, // TODO: remove after creating valid FE client in Keycloak
                RoleClaimType = "appRole"
            };
        });

        return services;
    }
}
