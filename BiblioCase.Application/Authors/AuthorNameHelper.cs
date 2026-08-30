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

    public static async Task<Author> GetOrCreateAuthorAsync(IAppDbContext db, string? authorName)
    {
        var normalizedName = Normalize(authorName);

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new InvalidOperationException("Author name cannot be empty.");
        }

        var comparisonKey = GetComparisonKey(normalizedName);

        var author = await db.Authors
            .FirstOrDefaultAsync(a =>
                a.Name.Trim() == comparisonKey ||
                a.Name.Trim().ToLower() == comparisonKey.ToLower());

        if (author is not null)
        {
            return author;
        }

        author = new Author
        {
            Name = normalizedName
        };

        db.Authors.Add(author);
        await db.SaveChangesAsync();

        return author;
    }
}
