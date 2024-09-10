namespace WebApiGraphQLClient.Response;

public class LoginResponse
{
	public LoginData Login { get; set; }
}

public class LoginData
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string Token { get; set; }
}
