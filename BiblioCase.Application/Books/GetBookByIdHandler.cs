using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class GetBookByIdHandler
{
    private readonly IAppDbContext _db;

    public GetBookByIdHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<BookDto?> Handle(int id)
    {
        return await _db.Books
            .Include(b => b.Author)
            .Where(b => b.Id == id)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author!.Name
            })
            .FirstOrDefaultAsync();
    }
}
