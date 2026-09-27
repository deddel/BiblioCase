using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public class UpdateAuthorHandler
{
    private readonly IAppDbContext _db;

    public UpdateAuthorHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<AuthorDto?> Handle(int id, UpdateAuthorRequest request)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author is null)
        {
            return null;
        }

        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return null;
        }

        author.FirstName = firstName;
        author.LastName = lastName;
        author.Biography = string.IsNullOrWhiteSpace(request.Biography) ? null : request.Biography.Trim();

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
