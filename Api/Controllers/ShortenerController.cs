using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Entities;

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

    [HttpGet("shorten/{code}")]
    public async Task<IActionResult> RedirectAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Code is empty or invalid");
        
        var result = await _shortenerService.ResolveOriginalUrlAndIncrementAsync(code);
        
        if (result == null) return NotFound(); 
        
        return Redirect(result);
    }

    [HttpGet("shorten/stats/{code}")]
    public async Task<ActionResult<UrlStatsDto>> GeetStatsAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Code is empty or invalid");
        
        var result = await _shortenerService.GetStatsAsync(code);
        
        if (result == null) return NotFound();
        
        return Ok(result);
    }

    [HttpGet("shorten/all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0) return BadRequest("page must be >= 1");
        if (pageSize <= 0 || pageSize > 100) return BadRequest("pageSize must be 1..100");
        
        var urls = await _shortenerService.GetPageAsync(page, pageSize);
        
        return Ok(urls);
    }
    
    [HttpDelete("shorten/{code}")]
    public async Task<IActionResult> DeleteAsync(string code)
    {
        var result = await _shortenerService.DeleteByCodeAsync(code);
        
        return result
            ? NoContent()
            : NotFound();
    }
}