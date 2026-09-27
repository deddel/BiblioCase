namespace BiblioCase.Application.DTOs;

public class UpdateAuthorRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
}
