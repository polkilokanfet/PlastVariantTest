using Lira.Shared.Requests;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PlastVariantTest.Authentication;


public class AuthenticationHttpMessageHandler : DelegatingHandler
{
    private readonly ITokenStore _tokenStore;
    private readonly HttpClient _httpClient;  // для вызова /refresh

    private static readonly SemaphoreSlim _refreshLock = new(1, 1); // защита от race condition

    public AuthenticationHttpMessageHandler(
        ITokenStore tokenStore,
        IHttpClientFactory httpClientFactory)
    {
        _tokenStore = tokenStore;
        _httpClient = httpClientFactory.CreateClient("RefreshClient");
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await SetAuthorizationHeaderAsync(request);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized &&
            await TryRefreshAccessTokenAsync())
        {
            await SetAuthorizationHeaderAsync(request);
            response = await base.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task SetAuthorizationHeaderAsync(HttpRequestMessage request)
    {
        var token = await _tokenStore.GetAccessTokenAsync();

        if (string.IsNullOrEmpty(token))
            // Если токена нет — убираем заголовок (на всякий случай)
            request.Headers.Authorization = null;
        else
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<bool> TryRefreshAccessTokenAsync()
    {
        var refreshToken = await this._tokenStore.GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken))
            return false;

        await _refreshLock.WaitAsync();
        try
        {
            var requestDto = new RefreshTokenRequest { RefreshToken = refreshToken };
            var json = JsonSerializer.Serialize(requestDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/auth/refresh", content);

            if (response.IsSuccessStatusCode == false)
                return false;

            var result = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(result))
                return false;

            await _tokenStore.SetAccessTokenAsync(result);
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _refreshLock.Release();
        }
    }
}