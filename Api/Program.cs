using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure.DbContext;
using UrlShortener.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ManagerDbCs")));
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IShortenerService>(sp =>
{
    var gen = sp.GetRequiredService<ICodeGenerator>();
    var baseUrl = builder.Configuration["Shortener:BaseUrl"]!;
    return new ShortenerService(gen, baseUrl);
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint
app.MapPost("/shorten", (ShortenRequest request, IShortenerService service) =>
    {
        var result = service.Shorten(request.Url);
        return Results.Ok(result);
    })
    .WithName("ShortenUrl");

app.Run();

