using GraphQL.Client.Http;
using System.Text.Json;
using WebApiGraphQLClient.GraphQl;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Response;

namespace WebApiGraphQLClient.Service;

public class UserService : IUserService
{
	private readonly GraphQLHttpClient _client;
	private readonly JsonSerializerOptions _options;

	public UserService(GraphQLHttpClient client)
	{
		_client = client;

		_options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

	}


	public async Task<AllUsersResponse?> GetAllUsersAsync()
	{

		var request = new GraphQLHttpRequest { Query = GetUsersQuery.GetAllUsers };

		var response = await _client.SendQueryAsync<AllUsersResponse>(request);

		var allUsersResponse = response.Data;

		return allUsersResponse!;
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
		return response.Data;

	}
}
