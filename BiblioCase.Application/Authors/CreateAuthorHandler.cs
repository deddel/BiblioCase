using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;

namespace BiblioCase.Application.Authors;

public class CreateAuthorHandler
{
    private readonly IAppDbContext _db;

    public CreateAuthorHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<AuthorDto?> Handle(CreateAuthorRequest request)
    {
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return null;
        }

        var author = new Author
        {
            FirstName = firstName,
            LastName = lastName,
            Biography = string.IsNullOrWhiteSpace(request.Biography) ? null : request.Biography.Trim()
        };

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Biography = author.Biography
        };
    }
}
