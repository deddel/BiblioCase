namespace BiblioCase.Application.DTOs;

public class UpdateBookRequest
{
    public string Title { get; set; } = "";
    public int? AuthorId { get; set; }
    public string? NewAuthorName { get; set; }
    public string AuthorName { get; set; } = "";
}
