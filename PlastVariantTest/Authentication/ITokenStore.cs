namespace PlastVariantTest.Authentication;

public interface ITokenStore
{
    ValueTask<string?> GetAccessTokenAsync();
    ValueTask SetAccessTokenAsync(string? token);
    ValueTask<string?> GetRefreshTokenAsync();
    ValueTask SetRefreshTokenAsync(string? token);
    ValueTask ClearAsync();
}
