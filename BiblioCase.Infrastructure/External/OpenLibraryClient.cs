using System.Net.Http.Json;

namespace BiblioCase.Infrastructure.External;

public class OpenLibraryClient
{
    private readonly HttpClient _httpClient;

    public OpenLibraryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri("https://openlibrary.org/");
    }

    public async Task<OpenLibrarySearchResult?> SearchAuthorsAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var url = $"search/authors.json?q={Uri.EscapeDataString(name)}";
        return await _httpClient.GetFromJsonAsync<OpenLibrarySearchResult>(url);
    }
}
