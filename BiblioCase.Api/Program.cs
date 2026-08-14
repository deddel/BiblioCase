using BiblioCase.Infrastructure;
using Microsoft.EntityFrameworkCore;
using BiblioCase.Application.Books;
using BiblioCase.Application.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GetBooksHandler>();
builder.Services.AddScoped<GetWeatherForecastHandler>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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


app.Run();
