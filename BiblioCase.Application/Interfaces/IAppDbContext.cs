using BiblioCase.Domain;
using Microsoft.EntityFrameworkCore;

namespace BiblioCase.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Book> Books { get; }
    DbSet<Author> Authors { get; }
    DbSet<Review> Reviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
