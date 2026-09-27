namespace BiblioCase.Application.DTOs;
public class AuthorBookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? FirstPublicationYear { get; set; }
}