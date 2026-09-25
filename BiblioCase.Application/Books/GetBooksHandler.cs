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
                Synopsis = b.Synopsis,
                FirstPublicationYear = b.FirstPublicationYear,
                Author = new AuthorDto
                {
                    Id = b.Author!.Id,
                    FirstName = b.Author.FirstName,
                    LastName = b.Author.LastName,
                    Biography = b.Author.Biography
                }
            })
            .ToListAsync();
    }
}