using System.Text.Json.Serialization;

namespace BiblioCase.Infrastructure.External;

public class OpenLibrarySearchResult
{
    public List<OpenLibraryAuthorDoc> Docs { get; set; } = new();
}

public class OpenLibraryAuthorDoc
{
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";

    [JsonPropertyName("birth_date")]
    public string? BirthDate { get; set; }

    [JsonPropertyName("top_work")]
    public string? TopWork { get; set; }
}
