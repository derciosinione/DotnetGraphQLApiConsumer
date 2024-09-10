using WebApiGraphQLClient.Response;

namespace WebApiGraphQLClient.Interface;

public interface IAuthService
{
    Task<LoginData> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    
    Task<string> GetInMemoryToken(CancellationToken cancellationToken = default);
}