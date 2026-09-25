using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public class GetAuthorsHandler
{
    private readonly IAppDbContext _db;

    public GetAuthorsHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AuthorDto>> Handle()
    {
        return await _db.Authors
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Biography = a.Biography
            })
            .ToListAsync();
    }
}
