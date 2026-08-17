using BiblioCase.Application.DTOs;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class GetBooksHandler
{
    private readonly AppDbContext _db;

    public GetBooksHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<BookDto>> Handle()
    {
        return await _db.Books
            .Include(b => b.Author)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author!.Name
            })
            .ToListAsync();
    }
}