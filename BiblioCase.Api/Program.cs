using BiblioCase.Application.Authors;
using BiblioCase.Application.Books;
using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using BiblioCase.Application.Weather;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Path to SQLite database
var dbPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "data", "BiblioCase.db"));

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddScoped<GetBooksHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<UpdateBookHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetAuthorsHandler>();
builder.Services.AddScoped<GetAuthorByIdHandler>();
builder.Services.AddScoped<CreateAuthorHandler>();
builder.Services.AddScoped<UpdateAuthorHandler>();
builder.Services.AddScoped<DeleteAuthorHandler>();
builder.Services.AddScoped<DeleteUnusedAuthorsHandler>();
builder.Services.AddScoped<GetWeatherForecastHandler>();
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (GetWeatherForecastHandler handler) =>
{
    var forecast = handler.Handle();
    return Results.Ok(forecast);
})
.WithName("GetWeatherForecast");

app.MapGet("/books", async (GetBooksHandler handler) =>
{
    var books = await handler.Handle();
    return Results.Ok(books);
});

app.MapGet("/books/{id:int}", async (int id, GetBookByIdHandler handler) =>
{
    var book = await handler.Handle(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.MapPost("/books", async (CreateBookRequest request, CreateBookHandler handler) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        (request.AuthorId is null &&
         (string.IsNullOrWhiteSpace(request.NewAuthorFirstName) ||
          string.IsNullOrWhiteSpace(request.NewAuthorLastName))))
    {
        return Results.BadRequest();
    }

    var result = await handler.Handle(request);

    if (result.AuthorNotFound)
    {
        return Results.NotFound();
    }

    if (result.Book is null)
    {
        return Results.BadRequest();
    }

    return Results.Created($"/books/{result.Book.Id}", result.Book);
});

app.MapPut("/books/{id:int}", async (int id, UpdateBookRequest request, UpdateBookHandler handler) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        (request.AuthorId is null &&
         (string.IsNullOrWhiteSpace(request.NewAuthorFirstName) ||
          string.IsNullOrWhiteSpace(request.NewAuthorLastName))))
    {
        return Results.BadRequest();
    }

    var book = await handler.Handle(id, request);

    if (book is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.MapDelete("/books/{id:int}", async (int id, DeleteBookHandler handler) =>
{
    var deleted = await handler.Handle(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.MapGet("/authors", async (GetAuthorsHandler handler) =>
{
    var authors = await handler.Handle();
    return Results.Ok(authors);
});

app.MapGet("/authors/{id:int}", async (int id, GetAuthorByIdHandler handler) =>
{
    var author = await handler.Handle(id);

    if (author is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(author);
});

app.MapPost("/authors", async (CreateAuthorRequest request, CreateAuthorHandler handler) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
    {
        return Results.BadRequest();
    }

    var author = await handler.Handle(request);

    if (author is null)
    {
        return Results.BadRequest();
    }

    return Results.Created($"/authors/{author.Id}", author);
});

app.MapPut("/authors/{id:int}", async (int id, UpdateAuthorRequest request, UpdateAuthorHandler handler) =>
{
    if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
    {
        return Results.BadRequest();
    }

    var author = await handler.Handle(id, request);

    if (author is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(author);
});

app.MapDelete("/authors/{id:int}", async (int id, DeleteAuthorHandler handler) =>
{
    var result = await handler.Handle(id);

    return result switch
    {
        DeleteAuthorResult.NotFound => Results.NotFound(),
        DeleteAuthorResult.InUse => Results.Conflict("Författaren har böcker kopplade till sig och kan inte tas bort."),
        _ => Results.NoContent()
    };
});

app.MapDelete("/authors/unused", async (DeleteUnusedAuthorsHandler handler) =>
{
    var removedCount = await handler.Handle();
    return Results.Ok(new { removedCount });
});

app.Run();
