using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.DTOs;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure.DbContext;
using UrlShortener.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddControllers();
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ManagerDbCS")));
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IShortenerService>(sp =>
{
    var gen = sp.GetRequiredService<ICodeGenerator>();
    var repo = sp.GetRequiredService<IShortUrlRepository>();
    var baseUrl = builder.Configuration["Shortener:BaseUrl"]!;
    return new ShortenerService(gen, repo, baseUrl);
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UrlShortenerDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

