using UrlShortener.Core.DTOs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Abstractions;

public interface IShortenerService
{
    Task<ShortenResult> ShortenAsync(string originalUrl);
    Task<UrlStatsDto?> GetStatsAsync(string code);
    Task<string?> ResolveOriginalUrlAndIncrementAsync(string code);
    Task<List<ShortUrl>> GetPageAsync(int page, int pageSize);
    Task<bool> DeleteByCodeAsync(string code);
}