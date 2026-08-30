using BiblioCase.Application.Authors;
using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Books;

public class UpdateBookHandler
{
    private readonly IAppDbContext _db;

    public UpdateBookHandler(IAppDbContext db)
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

        var oldAuthor = book.Author;

        book.Title = title;
        book.Author = author;

        if (oldAuthor is not null && oldAuthor.Id != author.Id)
        {
            var hasOtherBooks = await _db.Books
                .AnyAsync(b => b.AuthorId == oldAuthor.Id && b.Id != book.Id);

            if (!hasOtherBooks)
            {
                _db.Authors.Remove(oldAuthor);
            }
        }

        await _db.SaveChangesAsync();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = author.Name
        };
    }
}
