using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon;
using System.Text.Json;
using MoviesDataLoad.Models;
using System.Reflection;

internal class Program
{

    private static async Task Main(string[] args)
    {
        string tableName;

        if (args.Length > 0)
        {
            Console.WriteLine($"table name: {args[0]}");
            tableName = args[0];
        }
        else
        {
            Console.WriteLine("Error: any table name received");
            return;
        }

        Console.WriteLine("Initializing Setup");

        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
        clientConfig.RegionEndpoint = RegionEndpoint.USEast1;
        var client = new AmazonDynamoDBClient(clientConfig);

        Console.WriteLine("Check if table is empty");

        var scanRequest = new ScanRequest
        {
            TableName = tableName,
            Select = Select.COUNT
        };

        var response = await client.ScanAsync(scanRequest);

        if (response.Count > 0)
        {
            Console.WriteLine("Error: Table is not empty. Finishing...");
            return;
        }

        Console.WriteLine("Reading data.json file");

        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.json");
        var json = await File.ReadAllTextAsync(path);
        var data = JsonSerializer.Deserialize<MoviesData>(json);

        Console.WriteLine("Preparing to insert");

        foreach (var movie in data.Data)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["id"] = new AttributeValue { N = movie.Id.ToString() },
                ["name"] = new AttributeValue { S = movie.Name },
                ["image"] = new AttributeValue { S = movie.Image ?? "" },
                ["description"] = new AttributeValue { S = movie.Description ?? "" },
                ["type"] = new AttributeValue { S = movie.Type ?? "" },
                ["tags"] = new AttributeValue { SS = movie.Tags.ToList() }
            };

            var streamList = new List<AttributeValue>();

            foreach (var streamService in movie.Streams)
            {
                var streamItem = new Dictionary<string, AttributeValue>
                {
                    ["name"] = new AttributeValue { S = streamService.Name },
                    ["link"] = new AttributeValue { S = streamService.Link },
                };

                streamList.Add(new AttributeValue { M = streamItem });
            }


            item["streams"] = new AttributeValue { L = streamList };

            if (movie.Year.HasValue)
                item["year"] = new AttributeValue { N = movie.Year.Value.ToString() };

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };

            await client.PutItemAsync(request);
            Console.WriteLine($"Inserido: {movie.Name}");
        }
        
        Console.WriteLine($"Load Completed");
        
    }
}