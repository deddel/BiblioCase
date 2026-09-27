using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public class GetAuthorByIdHandler
{
    private readonly IAppDbContext _db;

    public GetAuthorByIdHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<AuthorDetailsDto?> Handle(int id)
    {
        return await _db.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDetailsDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Books = a.Books
                    .Select(b => new AuthorBookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        FirstPublicationYear = b.FirstPublicationYear
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}
