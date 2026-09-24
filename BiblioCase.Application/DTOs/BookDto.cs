namespace BiblioCase.Application.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Synopsis { get; set; }
    public int? FirstPublicationYear { get; set; }
    public AuthorDto Author { get; set; } = null!;
}