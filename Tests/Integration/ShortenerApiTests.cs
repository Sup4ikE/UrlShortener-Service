using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UrlShortener.Core.DTOs;
using Xunit;

namespace UrlShortener.Tests.Integration;

public class ShortenerApiTests: IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;
    
    public ShortenerApiTests(TestAppFactory factory)
    {
        _client = factory.CreateClient(new()
        {
            AllowAutoRedirect = false
        });
    }

    // [Fact]
    // public async Task Shorten_ThenRedirect_ThenStats_ClicksIncremented()
    // {
    //     // Arrange
    //     var originalUrl = "https://example.com";
    //     var request = new ShortenRequest(originalUrl);
    //     
    //     // Act 1 POST /shorten
    //     var postResp = await _client.PostAsJsonAsync("/api/Shortener/shorten", request);
    //     postResp.StatusCode.Should().Be(HttpStatusCode.OK);
    //     
    //     var shorten = await postResp.Content.ReadFromJsonAsync<ShortenResult>();
    //     shorten.Should().NotBeNull();
    //     shorten!.Code.Should().NotBeNullOrWhiteSpace();
    //     
    //     // Act 2: GET redirect
    //     var redirectResp = await _client.GetAsync($"/api/Shortener/shorten/{shorten.Code}");
    //     redirectResp.StatusCode.Should().Be(HttpStatusCode.Found);
    //     redirectResp.Headers.Location!.ToString().Should().Be(originalUrl);
    //
    //     // Act 3: GET stats
    //     var statsResp = await _client.GetAsync($"/api/Shortener/shorten/stats/{shorten.Code}");
    //     statsResp.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var stats = await statsResp.Content.ReadFromJsonAsync<UrlStatsDto>();
    //     stats.Should().NotBeNull();
    //     stats!.Code.Should().Be(shorten.Code);
    //     stats.OriginalUrl.Should().Be(originalUrl);
    //     stats.Clicks.Should().Be(1);
    // }
}