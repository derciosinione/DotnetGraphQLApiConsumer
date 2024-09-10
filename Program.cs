using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddHttpClient<GraphQLHttpClient>(client =>
{
	client.BaseAddress = new Uri("https://localhost:8081/graphql");
});

builder.Services.AddSingleton<GraphQLHttpClient>(sp =>
			new GraphQLHttpClient("https://localhost:8081/graphql",
				new SystemTextJsonSerializer()));

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
