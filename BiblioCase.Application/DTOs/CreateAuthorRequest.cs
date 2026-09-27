namespace BiblioCase.Application.DTOs;

public class CreateAuthorRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Biography { get; set; }
}
