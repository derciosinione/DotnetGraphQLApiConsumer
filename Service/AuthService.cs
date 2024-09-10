using Microsoft.Extensions.Caching.Memory;
using StrawberryShake;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Response;

namespace WebApiGraphQLClient.Service;

public class AuthService : IAuthService
{
    private readonly IR2YClient _client;
    private readonly IMemoryCache _memoryCache;

    public AuthService(IR2YClient client, IMemoryCache memoryCache)
    {
        _client = client;
        _memoryCache = memoryCache;
    }
    
    public async Task<LoginData> LoginAsync(string email, string password,
        CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue("AuthToken", out LoginData? cachedResponse))
            return cachedResponse!;
        
        var result = await _client.UserLogin.ExecuteAsync(email, password, cancellationToken);
        result.EnsureNoErrors();

        var data = result.Data!.Login;
        
        var token = new LoginData
        {
            Id = data.Id,
            Email = data.Email,
            Token = data.Token
        };
        
        _memoryCache.Set("AuthToken", token, new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(55)));
        
        return token;
    }

    public Task<string> GetInMemoryToken(CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue("AuthToken", out LoginData? cachedResponse))
        {
            return Task.FromResult(cachedResponse!.Token);
        }
        
        return Task.FromResult(string.Empty);
    }
}