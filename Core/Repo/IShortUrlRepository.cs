using UrlShortener.Core.DTOs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repo;

public interface IShortUrlRepository
{
    Task AddAsync(ShortUrl entity);
    Task<ShortUrl?> GetByCodeAsync(string code);
    Task<bool> ExistsByCodeAsync(string code);
    Task IncrementClicksAsync(string code);
    Task DeleteByCodeAsync(string code);
    Task<List<ShortUrl>> GetPageAsync(int page, int pageSize);
    Task SaveChangesAsync();
}

