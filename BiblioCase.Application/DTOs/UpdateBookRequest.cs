namespace BiblioCase.Application.DTOs;

public class UpdateBookRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Synopsis { get; set; }
    public int? FirstPublicationYear { get; set; }
    public int? AuthorId { get; set; }
    public string? AuthorBiography { get; set; }
    public string? NewAuthorFirstName { get; set; }
    public string? NewAuthorLastName { get; set; }
    public string? NewAuthorBiography { get; set; }
}
