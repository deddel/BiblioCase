namespace BiblioCase.Domain;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Synopsis { get; set; }

    public int? FirstPublicationYear { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
}