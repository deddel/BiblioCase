using BiblioCase.Application.DTOs;
using BiblioCase.Domain;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class CreateBookHandler
{
    private readonly AppDbContext _db;

    public CreateBookHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BookDto?> Handle(CreateBookRequest request)
    {
        var title = request.Title?.Trim() ?? "";
        var authorName = request.AuthorName?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(authorName))
        {
            return null;
        }

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

        var book = new Book
        {
            Title = title,
            Author = author
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = author.Name
        };
    }
}
