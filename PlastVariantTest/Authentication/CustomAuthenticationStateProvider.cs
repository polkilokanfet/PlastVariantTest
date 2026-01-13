using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Lira.Shared.Responses;
using Microsoft.AspNetCore.Components.Authorization;

namespace PlastVariantTest.Authentication;

public class CustomAuthenticationStateProvider(ITokenStore tokenStore) : AuthenticationStateProvider
{
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var accessToken = await tokenStore.GetAccessTokenAsync();
            var identity = accessToken == null
                ? new ClaimsIdentity()
                : GetClaimsIdentity(accessToken);
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
        catch (Exception)
        {
            await MarkUserAsLoggedOut();
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
    }

    public async Task MarkUserAsAuthenticated(LoginResponse response)
    {
        await tokenStore.SetAccessTokenAsync(response.AccessToken);
        await tokenStore.SetRefreshTokenAsync(response.RefreshToken);

        var identity = GetClaimsIdentity(response.AccessToken);
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims;
        return new ClaimsIdentity(claims, "jwt");
    }

    public async Task MarkUserAsLoggedOut()
    {
        await tokenStore.ClearAsync();
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}