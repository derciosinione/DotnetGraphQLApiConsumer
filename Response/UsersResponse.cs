namespace WebApiGraphQLClient.Response;

public class UsersResponse
{
}


public class User
{
	public string Id { get; set; }
	public string Email { get; set; }
}

public class AllUsersResponse
{
	public AllUsersData AllUsers { get; set; }
}

public class AllUsersData
{
	public User[] Nodes { get; set; }
}