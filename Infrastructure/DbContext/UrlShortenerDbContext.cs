using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;

namespace UrlShortener.Infrastructure.DbContext;

public class UrlShortenerDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }
    
    public DbSet<ShortUrl> ShortUrls { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>(e =>
        {
            e.HasIndex(x => x.Code).IsUnique();     
            e.Property(x => x.Code).IsRequired();
            e.Property(x => x.OriginalUrl).IsRequired();
        });
    }
}
