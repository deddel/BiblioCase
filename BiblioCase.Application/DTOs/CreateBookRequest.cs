namespace BiblioCase.Application.DTOs;

public class CreateBookRequest
{
    public string Title { get; set; } = "";
    public int? AuthorId { get; set; }
    public string? NewAuthorName { get; set; }
    public string AuthorName { get; set; } = "";
}
