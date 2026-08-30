using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class GetBooksHandler
{
    private readonly IAppDbContext _db;

    public GetBooksHandler(IAppDbContext db)
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