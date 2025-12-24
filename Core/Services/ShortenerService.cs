using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;

namespace UrlShortener.Core.Services;

public class ShortenerService: IShortenerService
{
    private readonly ICodeGenerator _generator;
    private readonly string _baseUrl;
    
    public ShortenerService(ICodeGenerator generator, string baseUrl)
    {
        _generator = generator;
        _baseUrl = baseUrl;
    }
    
    public ShortenResult Shorten(string originalUrl)
    {
        if(String.IsNullOrWhiteSpace(originalUrl))
        { 
            throw new ArgumentException("Url cannot be null", nameof(originalUrl));
        }
        
        string code = _generator.Generate();
        string shortUrl = $"{_baseUrl.TrimEnd('/')}/{code}";
        
        return new ShortenResult(code, shortUrl);
    }
}