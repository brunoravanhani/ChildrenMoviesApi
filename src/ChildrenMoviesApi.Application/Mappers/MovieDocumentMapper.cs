using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using ChildrenMoviesApi.Application.Models;

namespace ChildrenMoviesApi.Application.Mappers;

public static class MovieDocumentMapper
{
    public static Movie ToMovie(this Dictionary<string, AttributeValue> item)
    {
        var doc = Document.FromAttributeMap(item);

        var movie = new Movie
        {
            Id = doc.TryGetInt("id") ?? 0,
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

    // --- helpers seguros ---
    private static string GetString(this DynamoDBEntry entry) =>
        entry?.AsString();

    private static string TryGetString(this Document doc, string key)
    {
        return doc.TryGetValue(key, out var entry) ? entry.GetString() : null;
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
        if (doc.TryGetValue(key, out var entry) && entry is DynamoDBList list)
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
        if (doc.TryGetValue(key, out var entry) && entry is DynamoDBList list)
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
}
