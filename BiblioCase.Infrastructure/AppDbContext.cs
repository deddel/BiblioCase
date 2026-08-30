using BiblioCase.Application.Interfaces;
using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Review> Reviews => Set<Review>();
}