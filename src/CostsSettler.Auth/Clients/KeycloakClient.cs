﻿using CostsSettler.Auth.Config;
using CostsSettler.Auth.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CostsSettler.Auth.Clients;
public class KeycloakClient : IKeycloakClient
{
    private readonly HttpClient _httpClient;
    private readonly Uri _baseUri;
    private readonly string _realmName;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public KeycloakClient(KeycloakClientConfig config, bool ignoreSSL = false)
    {
        var handler = new HttpClientHandler();

        if (ignoreSSL)
        {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
        }

        _httpClient = new HttpClient(handler);

        _baseUri = new Uri(config.AuthServerUrl);
        _realmName = config.Realm;
        _clientSecret = config.Secret;
        _clientId = config.ClientId;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        var content = new Dictionary<string, string>
            {
                { "client_secret", _clientSecret },
                { "client_id", _clientId },
                { "grant_type", "client_credentials" }
            };

        var response = await _httpClient.PostAsync(
            new Uri(_baseUri, $"realms/{_realmName}/protocol/openid-connect/token"),
            new FormUrlEncodedContent(content)
        );

        var keycloakAccessToken = JsonConvert.DeserializeObject<KcAccessToken>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }
            });

        return keycloakAccessToken?.AccessToken;
    }

    public async Task<ICollection<KcUser>?> GetUsersAsync(string accessToken)
    {
        var request = CreateRequest(
            HttpMethod.Get,
            $"admin/realms/{_realmName}/users",
            accessToken
        );

        var users = await SendRequestAsync<List<KcUser>>(request);

        return users;
    }

    public async Task<KcUser?> GetUserByIdAsync(Guid userId, string accessToken)
    {
        var request = CreateRequest(
            HttpMethod.Get,
            $"admin/realms/{_realmName}/users/{userId}",
            accessToken
        );

        var user = await SendRequestAsync<KcUser>(request);

        return user;
    }

    private HttpRequestMessage CreateRequest(HttpMethod httpMethod, string relativeUri, string accessToken, StringContent? content = null)
    {
        var request = new HttpRequestMessage()
        {
            Method = httpMethod,
            RequestUri = new Uri(_baseUri, relativeUri)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        if (content is not null)
            request.Content = content;

        return request;
    }

    private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request)
        where T : new()
    {
        T? result = default;
        await _httpClient.SendAsync(request)
            .ContinueWith(response =>
            {
                result = JsonConvert.DeserializeObject<T>(response.Result.Content.ReadAsStringAsync().Result);
            });

        return result;
    }
}