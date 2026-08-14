using BiblioCase.Application.DTOs;
using BiblioCase.Domain;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class UpdateBookHandler
{
    private readonly AppDbContext _db;

    public UpdateBookHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BookDto?> Handle(int id, UpdateBookRequest request)
    {
        var book = await _db.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null)
        {
            return null;
        }

        var title = request.Title?.Trim() ?? "";
        var authorName = request.AuthorName?.Trim() ?? "";

        var normalizedAuthorName = authorName.ToLower();
        var author = await _db.Authors
            .FirstOrDefaultAsync(a => a.Name.ToLower() == normalizedAuthorName);

        if (author is null)
        {
            author = new Author
            {
                Name = authorName
            };
        }

        book.Title = title;
        book.Author = author;

        await _db.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = author.Name
        };
    }
}
