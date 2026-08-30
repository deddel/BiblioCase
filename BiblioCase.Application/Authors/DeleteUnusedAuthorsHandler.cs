using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public class DeleteUnusedAuthorsHandler
{
    private readonly IAppDbContext _db;

    public DeleteUnusedAuthorsHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle()
    {
        var authorsToDelete = await _db.Authors
            .Where(a => !_db.Books.Any(b => b.AuthorId == a.Id))
            .ToListAsync();

        _db.Authors.RemoveRange(authorsToDelete);
        await _db.SaveChangesAsync();

        return authorsToDelete.Count;
    }
}
