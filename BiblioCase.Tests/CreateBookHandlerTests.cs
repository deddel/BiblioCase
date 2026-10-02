using BiblioCase.Application.Books;
using BiblioCase.Application.DTOs;
using BiblioCase.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Tests;

public class CreateBookHandlerTests
{
    [Fact]
    public async Task Handle_CreatesBookForExistingAuthor()
    {
        await using var connection = CreateOpenConnection();
        await using var db = CreateDbContext(connection);
        var author = new BiblioCase.Domain.Author
        {
            FirstName = "Octavia",
            LastName = "Butler"
        };
        db.Authors.Add(author);
        await db.SaveChangesAsync();

        var result = await new CreateBookHandler(db).Handle(new CreateBookRequest
        {
            Title = "Kindred",
            AuthorId = author.Id
        });

        Assert.False(result.AuthorNotFound);
        Assert.NotNull(result.Book);
        Assert.Equal("Kindred", result.Book.Title);
        Assert.Equal(author.Id, result.Book.Author.Id);
    }

    [Fact]
    public async Task Handle_ReturnsAuthorNotFound_WhenSuppliedAuthorDoesNotExist()
    {
        await using var connection = CreateOpenConnection();
        await using var db = CreateDbContext(connection);

        var result = await new CreateBookHandler(db).Handle(new CreateBookRequest
        {
            Title = "Kindred",
            AuthorId = 42
        });

        Assert.True(result.AuthorNotFound);
        Assert.Null(result.Book);
        Assert.Empty(db.Books);
    }

    [Fact]
    public async Task Handle_ReturnsInvalidResult_WhenNewAuthorNameIsIncomplete()
    {
        await using var connection = CreateOpenConnection();
        await using var db = CreateDbContext(connection);

        var result = await new CreateBookHandler(db).Handle(new CreateBookRequest
        {
            Title = "Kindred",
            NewAuthorFirstName = "Octavia"
        });

        Assert.False(result.AuthorNotFound);
        Assert.Null(result.Book);
    }

    private static SqliteConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();
        return connection;
    }

    private static AppDbContext CreateDbContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }
}