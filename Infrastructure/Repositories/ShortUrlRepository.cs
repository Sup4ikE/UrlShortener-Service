using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Abstractions;
using UrlShortener.Infrastructure.DbContext;

namespace UrlShortener.Infrastructure.Repositories;

public class ShortUrlRepository: IShortUrlRepository
{
    private readonly UrlShortenerDbContext _dbContext;
    
    public ShortUrlRepository(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddAsync(ShortUrl entity)
    {
        await _dbContext.ShortUrls.AddAsync(entity);
    }

    public async Task<ShortUrl?> GetByCodeAsync(string code)
    {
        return await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Code == code);
    }

    public Task<bool> ExistsByCodeAsync(string code)
    {
        return _dbContext.ShortUrls.AnyAsync(x => x.Code == code);
    }

    public async Task IncrementClicksAsync(string code)
    {
        var entity = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Code == code);
        if (entity == null) return;
    
        entity.Clicks++;
    }

    public async Task DeleteByCodeAsync(string code)
    {
        var entity = await _dbContext.ShortUrls.FirstOrDefaultAsync(x => x.Code == code);
        
        if (entity == null) return;
        
        _dbContext.ShortUrls.Remove(entity);
    }

    public async Task<List<ShortUrl>> GetPageAsync(int page, int pageSize)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        return await _dbContext.ShortUrls
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task SaveChangesAsync()
    { 
        await _dbContext.SaveChangesAsync();
    }
}
