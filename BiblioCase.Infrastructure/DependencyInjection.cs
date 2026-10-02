using BiblioCase.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BiblioCase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            options.UseSqlite(connectionString));

        return services;
    }
}
