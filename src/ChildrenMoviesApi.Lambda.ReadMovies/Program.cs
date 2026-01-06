using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Configuration;
using ChildrenMoviesApi.Domain.Mappers;
using Microsoft.Extensions.Options;

var specificOrigins = "_specificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseTables>(builder.Configuration.GetSection("Database"));

var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: specificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins(allowedOrigins!);
                          policy.WithMethods("GET");
                      });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(specificOrigins);

app.MapGet("/", async (IOptions<DatabaseTables> options) =>
{
    var dynamoDbClient = new AmazonDynamoDBClient();

    var scanRequest = new ScanRequest
    {
        TableName = options.Value.Movie
    };

    var response = await dynamoDbClient.ScanAsync(scanRequest);

    return response.Items.Select(MovieDocumentMapper.ToMovie).ToList();
})
.WithName("ReadMovies");

app.Run();

