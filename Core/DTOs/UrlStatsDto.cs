namespace UrlShortener.Core.DTOs;

public record UrlStatsDto(string Code, string OriginalUrl, DateTime CreatedAt, int Clicks);