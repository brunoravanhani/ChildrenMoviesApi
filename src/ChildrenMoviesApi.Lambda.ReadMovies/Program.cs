using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Configuration;
using ChildrenMoviesApi.Domain.Mappers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.Configure<DatabaseTables>(builder.Configuration.GetSection("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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

