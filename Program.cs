using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddHttpClient<GraphQLHttpClient>(client =>
{
	client.BaseAddress = new Uri("https://localhost:8081/graphql");
});

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<GraphQLHttpClient>(sp =>
			new GraphQLHttpClient("https://localhost:8081/graphql",
				new SystemTextJsonSerializer()));

builder.Services
	.AddR2YClient()
	.ConfigureHttpClient((sp, client) =>
	{
		client.BaseAddress = new Uri("https://localhost:8081/graphql");
        
		// Get token only when needed (for resolvers that require authentication)
		var tokenService = sp.GetRequiredService<IAuthService>();
		var token = tokenService.GetInMemoryToken().Result;
		
		// Conditionally add the Authorization header
		if (!string.IsNullOrEmpty(token))
		{
			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
		}
	});

// IServiceProvider services = serviceCollection.BuildServiceProvider();
// IConferenceClient client = services.GetRequiredService<IConferenceClient>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
