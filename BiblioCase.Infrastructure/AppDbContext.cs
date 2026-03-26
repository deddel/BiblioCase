using Microsoft.EntityFrameworkCore;
using BiblioCase.Domain;

namespace BiblioCase.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Review> Reviews => Set<Review>();
}