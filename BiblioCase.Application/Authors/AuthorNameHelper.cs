using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Authors;

public static class AuthorNameHelper
{
    public static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return string.Join(' ', value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    public static string GetComparisonKey(string? value)
    {
        return Normalize(value).Trim();
    }

    public static (string FirstName, string LastName) SplitName(string? value)
    {
        var normalizedName = Normalize(value);
        var separatorIndex = normalizedName.IndexOf(' ');

        return separatorIndex < 0
            ? (normalizedName, string.Empty)
            : (normalizedName[..separatorIndex], normalizedName[(separatorIndex + 1)..]);
    }

    public static string FormatName(Author author)
    {
        return Normalize($"{author.FirstName} {author.LastName}");
    }

    public static async Task<Author> GetOrCreateAuthorAsync(IAppDbContext db, string? authorName)
    {
        var normalizedName = Normalize(authorName);

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Author name cannot be empty.");
        }

        var (firstName, lastName) = SplitName(normalizedName);

        var author = await db.Authors
            .FirstOrDefaultAsync(a =>
            a.FirstName.Trim().ToLower() == firstName.ToLower() &&
            a.LastName.Trim().ToLower() == lastName.ToLower());

        if (author is not null)
        {
            return author;
        }

        author = new Author
        {
            FirstName = firstName,
            LastName = lastName
        };

        db.Authors.Add(author);
        await db.SaveChangesAsync();

        return author;
    }
}
