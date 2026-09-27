namespace BiblioCase.Application.DTOs;

public class AuthorDetailsDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<AuthorBookDto> Books { get; set; } = new();
}