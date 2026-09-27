using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public enum DeleteAuthorResult
{
    NotFound,
    InUse,
    Deleted
}

public class DeleteAuthorHandler
{
    private readonly IAppDbContext _db;

    public DeleteAuthorHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<DeleteAuthorResult> Handle(int id)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (author is null)
        {
            return DeleteAuthorResult.NotFound;
        }

        var isInUse = await _db.Books.AnyAsync(b => b.AuthorId == id);

        if (isInUse)
        {
            return DeleteAuthorResult.InUse;
        }

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();

        return DeleteAuthorResult.Deleted;
    }
}
