namespace PlastVariantTest.Authentication;

public class InMemoryAccessTokenStore : ITokenStore
{
    private string? _currentAccessToken;
    private string? _currentRefreshToken;

    public ValueTask<string?> GetAccessTokenAsync() => new ValueTask<string?>(_currentAccessToken);

    public ValueTask SetAccessTokenAsync(string? token)
    {
        _currentAccessToken = token;
        return default;
    }

    public ValueTask<string?> GetRefreshTokenAsync() => new ValueTask<string?>(_currentRefreshToken);

    public ValueTask SetRefreshTokenAsync(string? token)
    {
        _currentRefreshToken = token;
        return default;
    }

    public ValueTask ClearAsync()
    {
        _currentAccessToken = null;
        _currentRefreshToken = null;
        return default;
    }
}
