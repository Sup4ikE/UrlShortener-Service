using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ShortenerController: ControllerBase
{
    private readonly IShortenerService _shortenerService;

    public ShortenerController(IShortenerService shortenerService)
    {
        _shortenerService = shortenerService;
    }

    [HttpPost("shorten")]
    public async Task<ActionResult<ShortenResult>> Shorten([FromBody] ShortenRequest request)
    {
        var result = await _shortenerService.ShortenAsync(request.Url);
        return Ok(result);
    }
}