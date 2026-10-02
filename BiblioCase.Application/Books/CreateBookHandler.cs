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

    public async Task<CreateBookResult> Handle(CreateBookRequest request)
    {
        var title = request.Title.Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            return new CreateBookResult(null);
        }

        Author author;

        if (request.AuthorId is > 0)
        {
            var existingAuthor = await _db.Authors
                .FirstOrDefaultAsync(a => a.Id == request.AuthorId);

            if (existingAuthor is null)
            {
                return new CreateBookResult(null, AuthorNotFound: true);
            }

            author = existingAuthor;
        }
        else
        {
            var firstName = request.NewAuthorFirstName?.Trim();
            var lastName = request.NewAuthorLastName?.Trim();

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName))
            {
                return new CreateBookResult(null);
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

        var book = new Book
        {
            Title = title,
            Synopsis = request.Synopsis,
            FirstPublicationYear = request.FirstPublicationYear,
            Author = author
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        return new CreateBookResult(new BookDto
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
        });
    }
}
