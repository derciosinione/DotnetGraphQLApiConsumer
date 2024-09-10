namespace WebApiGraphQLClient.GraphQl;

public class GetUsersQuery
{
	public const string GetAllUsers = @"
		query {
		  allUsers(first: 10) {
			nodes {
			  id
			  email
			}
		  }
		}
	";
}
