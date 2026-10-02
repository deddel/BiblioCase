using BiblioCase.Application.Authors;
using BiblioCase.Application.Books;
using Microsoft.Extensions.DependencyInjection;

namespace BiblioCase.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetBooksHandler>();
        services.AddScoped<GetBookByIdHandler>();
        services.AddScoped<CreateBookHandler>();
        services.AddScoped<UpdateBookHandler>();
        services.AddScoped<DeleteBookHandler>();
        services.AddScoped<GetAuthorsHandler>();
        services.AddScoped<GetAuthorByIdHandler>();
        services.AddScoped<CreateAuthorHandler>();
        services.AddScoped<UpdateAuthorHandler>();
        services.AddScoped<DeleteAuthorHandler>();
        services.AddScoped<DeleteUnusedAuthorsHandler>();

        return services;
    }
}
