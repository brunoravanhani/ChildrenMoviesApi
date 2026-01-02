using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Infrastructure.Mappers;

public static class MovieDocumentMapper
{
    public static Movie ToMovie(this Dictionary<string, AttributeValue> item)
    {
        var doc = Document.FromAttributeMap(item);

        var movie = new Movie
        {
            Id = doc.TryGetGuid("id") ?? Guid.Empty,
            Name = doc.TryGetString("name"),
            Image = doc.TryGetString("image"),
            Description = doc.TryGetString("description"),
            Year = doc.TryGetInt("year"),
            Type = doc.TryGetString("type"),
            Streams = doc.TryGetStreams("streams"),
            Tags = doc.TryGetStringList("tags")
        };

        return movie;
    }

    public static Dictionary<string, AttributeValue> ToDynamoObject(this Movie movie)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "id", new AttributeValue { S = movie.Id.ToString() } },
            { "name", new AttributeValue { S = movie.Name } },
            { "image", new AttributeValue { S = movie.Image } },
            { "description", new AttributeValue { S = movie.Description } },
            { "year", new AttributeValue { N = movie.Year.ToString() } },
            { "type", new AttributeValue { S = movie.Type } },
            { "streams", new AttributeValue { 
                L = MapSteamsToDynamo(movie.Streams)} 
            },
            { "tags", new AttributeValue { SS =  movie.Tags.ToList()  } },
        };

        return item;
    }

    // --- helpers seguros ---
    private static string GetString(this DynamoDBEntry entry) =>
        entry?.AsString();
    
    private static string TryGetString(this Document doc, string key)
    {
        return doc.TryGetValue(key, out var entry) ? entry.GetString() : null;
    }

    private static Guid? TryGetGuid(this Document doc, string key)
    {
        return doc.TryGetValue(key, out var entry) ? Guid.Parse(entry.GetString()) : null;
    }

    private static int? TryGetInt(this Document doc, string key)
    {
        if (doc.TryGetValue(key, out var entry) && entry != null)
        {
            try { return entry.AsInt(); }
            catch { return null; }
        }
        return null;
    }

    private static IEnumerable<string> TryGetStringList(this Document doc, string key)
    {
        if (doc.TryGetValue(key, out var entry) && entry is PrimitiveList list)
        {
            return list.Entries
                       .Where(e => e != null)
                       .Select(e => e.AsString())
                       .ToList();
        }
        return Enumerable.Empty<string>();
    }

    private static IEnumerable<StreamService> TryGetStreams(this Document doc, string key)
    {
        doc.TryGetValue(key, out var entry);

        if (entry is DynamoDBList list)
        {
            return list.Entries
                       .OfType<Document>()
                       .Select(e => new StreamService
                       {
                           Name = e.TryGetString("name"),
                           Link = e.TryGetString("link")
                       })
                       .ToList();
        }
        return Enumerable.Empty<StreamService>();
    }

    private static List<AttributeValue> MapSteamsToDynamo(IEnumerable<StreamService> streams)
    {
        return streams.Select(s =>
        {
           return new AttributeValue
           {
               M = new Dictionary<string, AttributeValue>
               {
                   { "name", new AttributeValue { S = s.Name } },
                   { "link", new AttributeValue { S = s.Link } },
               } 
           };
        }).ToList();
    }
}
