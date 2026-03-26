namespace BiblioCase.Domain;

public class Review
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int Rating { get; set; } // 1–5

    public string Comment { get; set; } = string.Empty;
}