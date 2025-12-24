using UrlShortener.Core.DTOs;
namespace UrlShortener.Core.Abstractions;

public interface IShortenerService
{
    ShortenResult Shorten(string originalUrl);
}