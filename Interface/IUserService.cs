
using WebApiGraphQLClient.Response;

namespace WebApiGraphQLClient.Interface;

public interface IUserService
{
	Task<AllUsersResponse?> GetAllUsersAsync();
	Task<AllUsersResponse?> GetAllUsersWithAuthAsync(string token);
	Task<LoginResponse> LoginAsync(string email, string password);
}
