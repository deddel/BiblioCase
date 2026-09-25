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

        var title = request.Title.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            return null;
        }

        Author author;

        if (request.AuthorId is > 0)
        {
            author = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == request.AuthorId)
                ?? throw new InvalidOperationException("Author not found.");

            if (request.AuthorBiography is not null)
            {
                author.Biography = string.IsNullOrWhiteSpace(request.AuthorBiography)
                    ? null
                    : request.AuthorBiography.Trim();
            }
        }
        else
        {
            var firstName = request.NewAuthorFirstName?.Trim();
            var lastName = request.NewAuthorLastName?.Trim();

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName))
            {
                return null;
            }

            author = await _db.Authors
                .FirstOrDefaultAsync(a =>
                    a.FirstName.Trim().ToLower() == firstName.ToLower() &&
                    a.LastName.Trim().ToLower() == lastName.ToLower())
                ?? new Author
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Biography = request.NewAuthorBiography?.Trim()
                };

            if (author.Id == 0)
            {
                _db.Authors.Add(author);
            }
        }

        var oldAuthor = book.Author;

        book.Title = title;
        book.Synopsis = request.Synopsis;
        book.FirstPublicationYear = request.FirstPublicationYear;
        book.Author = author;

        if (oldAuthor.Id != author.Id)
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
            Synopsis = book.Synopsis,
            FirstPublicationYear = book.FirstPublicationYear,
            Author = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Biography = author.Biography
            }
        };
    }
}
