using BiblioCase.Application.Authors;
using BiblioCase.Application.Books;
using BiblioCase.Application.DTOs;
using BiblioCase.Application.Interfaces;
using BiblioCase.Application.Weather;
using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApi();
builder.Services.AddScoped<GetBooksHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<UpdateBookHandler>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<GetAuthorsHandler>();
builder.Services.AddScoped<GetWeatherForecastHandler>();
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
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
        (request.AuthorId is null && string.IsNullOrWhiteSpace(request.NewAuthorName) && string.IsNullOrWhiteSpace(request.AuthorName)))
    {
        return Results.BadRequest();
    }

    var book = await handler.Handle(request);

    if (book is null)
    {
        return Results.BadRequest();
    }

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id:int}", async (int id, UpdateBookRequest request, UpdateBookHandler handler) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        (request.AuthorId is null && string.IsNullOrWhiteSpace(request.NewAuthorName) && string.IsNullOrWhiteSpace(request.AuthorName)))
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


app.Run();
