using CostsSettler.Auth.Clients;
using CostsSettler.Auth.Config;
using CostsSettler.Domain.Interfaces.Repositories;
using CostsSettler.Domain.Services;
using CostsSettler.Repo.Repositories;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.IdentityModel.Tokens;

namespace CostsSettler.API.Extensions;

/// <summary>
/// 'IServiceCollection' interface extension methods.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services needed to start the application.
    /// </summary>
    /// <param name="services">Service collection that application services are added to.</param>
    /// <returns>Given 'services' object with services added.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICircumstanceRepository, CircumstanceRepository>();
        services.AddScoped<IChargeRepository, ChargeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }

    /// <summary>
    /// Adds 'KeycloakClient' to service collection.
    /// </summary>
    /// <param name="services">Service collection that application services are added to.</param>
    /// <param name="config">KeycloakClient configuration.</param>
    /// <param name="ignoreSSL">SSL requirement should be ignored (true) or not (false).</param>
    /// <returns>Given 'services' object with services added.</returns>
    public static IServiceCollection AddKeycloakClient(this IServiceCollection services, KeycloakClientConfig config, bool ignoreSSL = false)
    {
        var keycloakClient = new KeycloakClient(config, ignoreSSL);

        services.AddScoped<IKeycloakClient>((serviceProvider) => keycloakClient);

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services">Service collection that application services are added to.</param>
    /// <param name="config">Configuration for given service collection. 
    /// Should have 'KeycloakClientConfig' section with keycloak configuration fields.</param>
    /// <param name="env">Environment for given service collection.</param>
    /// <returns>Given 'services' object with services and authentication added.</returns>
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
                ValidateAudience = false,
                RoleClaimType = "appRole"
            };
        });

        return services;
    }
}
