using BiblioCase.Infrastructure;

namespace BiblioCase.Application.Books;

public class DeleteBookHandler
{
    private readonly AppDbContext _db;

    public DeleteBookHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(int id)
    {
        var book = await _db.Books.FindAsync(id);

        if (book is null)
        {
            return false;
        }

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();

        return true;
    }
}
