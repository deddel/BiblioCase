namespace BiblioCase.Domain;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; } 
    public List<Book> Books { get; set; } = new();
}