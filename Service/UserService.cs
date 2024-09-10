using GraphQL.Client.Http;
using System.Text.Json;
using GraphQL;
using Microsoft.Extensions.Caching.Memory;
using StrawberryShake;
using WebApiGraphQLClient.GraphQl;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Response;

namespace WebApiGraphQLClient.Service;

public class UserService : IUserService
{
	private readonly GraphQLHttpClient _client;
	private readonly IR2YClient _r2YClient;
	private readonly IMemoryCache _cache;

	public UserService(GraphQLHttpClient client, IR2YClient r2YClient, IMemoryCache cache)
	{
		_client = client;
		_r2YClient = r2YClient;
		_cache = cache;
	}


	public async Task<AllUsersResponse?> GetAllUsersAsync()
	{

		var request = new GraphQLHttpRequest { Query = GetUsersQuery.GetAllUsers };

		var response = await _client.SendQueryAsync<AllUsersResponse>(request);

		var allUsersResponse = response.Data;

		return allUsersResponse!;
	}

	public async Task< IReadOnlyList<IGetAllUsers_AllUsers_Nodes>?> GetUsersWithStrawberryShake()
	{
		var result = await _r2YClient.GetAllUsers.ExecuteAsync();
		result.EnsureNoErrors();
		
		return result.Data!.AllUsers!.Nodes;
	}

	public async Task<AllUsersResponse?> GetAllUsersWithAuthAsync(string token)
	{

		var request = new GraphQLHttpRequest { Query = GetUsersQuery.GetAllUsers };

		_client.HttpClient.DefaultRequestHeaders.Authorization =
		new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

		var response = await _client.SendQueryAsync<AllUsersResponse>(request);

		var allUsersResponse = response.Data;

		return allUsersResponse!;
	}

	public async Task<LoginResponse> LoginAsync(string email, string password)
	{
		// Check if the token is already cached
		if (_cache.TryGetValue("AuthToken", out LoginResponse cachedResponse))
		{
			return cachedResponse;
		}

		var request = new GraphQLHttpRequest
		{
			Query = @"
            mutation MakeLogin($email: String!, $password: String!) {
                login(email: $email, password: $password) {
                    id
                    email
                    token
                }
            }",
			Variables = new
			{
				email,
				password
			}
		};

		var response = await _client.SendMutationAsync<LoginResponse>(request);

		// Cache the result with an expiration time
		var cacheEntryOptions = new MemoryCacheEntryOptions()
			.SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); // Adjust as needed

		_cache.Set("AuthToken", response.Data, cacheEntryOptions);

		return response.Data;
	}
}
