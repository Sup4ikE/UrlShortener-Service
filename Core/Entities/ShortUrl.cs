namespace UrlShortener.Core.Entities;

public class ShortUrl
{
    public int Id { get; set; }
    
    public required string Code { get; set;}
    public required string OriginalUrl { get; set;}
    
    public DateTime CreatedAt { get; set;} = DateTime.UtcNow;
    public int Clicks { get; set;}
}

