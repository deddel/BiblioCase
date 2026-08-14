using BiblioCase.Application.DTOs;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public class GetAuthorsHandler
{
    private readonly AppDbContext _db;

    public GetAuthorsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<AuthorDto>> Handle()
    {
        return await _db.Authors
            .OrderBy(a => a.Name)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync();
    }
}
