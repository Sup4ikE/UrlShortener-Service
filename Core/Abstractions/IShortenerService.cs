using UrlShortener.Core.DTOs;
namespace UrlShortener.Core.Abstractions;

public interface IShortenerService
{
    Task<ShortenResult> ShortenAsync(string originalUrl);
}