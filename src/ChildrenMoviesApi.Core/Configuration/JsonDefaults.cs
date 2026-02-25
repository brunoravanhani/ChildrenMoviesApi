using System.Text.Json;

namespace ChildrenMoviesApi.Core.Configuration;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
