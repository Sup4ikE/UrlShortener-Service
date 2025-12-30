using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Services;

public class ShortenerService: IShortenerService
{
    private readonly ICodeGenerator _generator;
    private readonly IShortUrlRepository _repo;
    private readonly string _baseUrl;
    
    public ShortenerService(ICodeGenerator generator, IShortUrlRepository repo, string baseUrl)
    {
        _generator = generator;
        _baseUrl = baseUrl;
        _repo = repo;
    }
    
    
    public async Task<ShortenResult> ShortenAsync(string originalUrl)
    {
        if (string.IsNullOrWhiteSpace(originalUrl) || !Uri.TryCreate(originalUrl, UriKind.Absolute, out var resultUri) || (resultUri.Scheme != Uri.UriSchemeHttp && resultUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Invalid URL", nameof(originalUrl));
        }
        
        bool exists = false;
        string code = string.Empty;
        
        for (int i = 0; i <= 11; i++)
        {
            if (i == 11)
            {
                throw new Exception("Unable to generate unique code");
            }
            
            code = _generator.Generate();
            exists = await _repo.ExistsByCodeAsync(code);
            if (!exists) break;
        }

        ShortUrl shortUrl = new ShortUrl()
        {
            Code = code,
            OriginalUrl = originalUrl,
            Clicks = 0
        };
        
        await _repo.AddAsync(shortUrl);
        await _repo.SaveChangesAsync();
        
        string shortRes = $"{_baseUrl.TrimEnd('/')}/{code}";
        return new ShortenResult(code, shortRes);
    }
    
    public async Task<string?> ResolveOriginalUrlAndIncrementAsync(string code)
    {
        if (code == null || code == string.Empty) return null;
        
        var entity = await _repo.GetByCodeAsync(code);
        if (entity == null) return null;
            
        await _repo.IncrementClicksAsync(code);
        await _repo.SaveChangesAsync();
        
        return entity.OriginalUrl;
    }
    
    public async Task<UrlStatsDto?> GetStatsAsync(string code)
    {
        if (code == null || code == string.Empty) return null;
        
        var entity = await _repo.GetByCodeAsync(code);
        if (entity == null) return null;

        var ent = new UrlStatsDto(
            entity.Code,
            entity.OriginalUrl,
            entity.CreatedAt,
            entity.Clicks
            );
        
        return ent;
    }
    
    public async Task<List<ShortUrl>> GetPageAsync(int page, int pageSize) => await _repo.GetPageAsync(page, pageSize);

    public async Task<bool> DeleteByCodeAsync(string code)
    {
        if (code == null || code == string.Empty) return false;
        
        var entity = await _repo.GetByCodeAsync(code);
        
        if(entity == null) return false;
        
        await _repo.DeleteByCodeAsync(code);
        await _repo.SaveChangesAsync();
        return true;
    }
}