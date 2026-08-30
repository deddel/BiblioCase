using BiblioCase.Application.Authors;
using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class CreateBookHandler
{
    private readonly IAppDbContext _db;

    public CreateBookHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<BookDto?> Handle(CreateBookRequest request)
    {
        var title = AuthorNameHelper.Normalize(request.Title);

        if (string.IsNullOrWhiteSpace(title))
        {
            return null;
        }

        Author author;

        if (!string.IsNullOrWhiteSpace(request.NewAuthorName))
        {
            author = await AuthorNameHelper.GetOrCreateAuthorAsync(_db, request.NewAuthorName);
        }
        else if (request.AuthorId is > 0)
        {
            author = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == request.AuthorId)
                ?? throw new InvalidOperationException("Author not found.");
        }
        else
        {
            var authorName = AuthorNameHelper.Normalize(request.AuthorName);

            if (string.IsNullOrWhiteSpace(authorName))
            {
                return null;
            }

            author = await AuthorNameHelper.GetOrCreateAuthorAsync(_db, authorName);
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
