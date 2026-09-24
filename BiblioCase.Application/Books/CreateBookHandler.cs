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
                    a.FirstName == firstName &&
                    a.LastName == lastName)
                ?? new Author
                {
                    FirstName = firstName,
                    LastName = lastName
                };

            if (author.Id == 0)
            {
                _db.Authors.Add(author);
            }
        }

        var book = new Book
        {
            Title = title,
            Synopsis = request.Synopsis,
            FirstPublicationYear = request.FirstPublicationYear,
            Author = author
        };

        _db.Books.Add(book);
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
                LastName = author.LastName
            }
        };
    }
}
