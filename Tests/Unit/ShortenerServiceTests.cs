using FluentAssertions;
using Moq;
using UrlShortener.Core.Abstractions;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Services;
using Xunit;

namespace UrlShortener.Tests.Unit;

public class ShortenerServiceTests
{
    [Fact]
    public async Task ShortenAsync_ValidUrl_SavesEntityAndReturnsResult()
    {
        // Arrange
        var genMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        
        genMock.Setup(g => g.Generate(It.IsAny<int>())).Returns("ABC123");
        repoMock.Setup(r => r.ExistsByCodeAsync("ABC123")).ReturnsAsync(false);

        var service = new ShortenerService(genMock.Object, repoMock.Object, baseUrl);
        
        // Act
        var result = await service.ShortenAsync("https://google.com");
        
        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("ABC123");
        result.ShortUrl.Should().Be("http://localhost:5070/ABC123");
        
        repoMock.Verify(r => r.AddAsync(It.Is<ShortUrl>(x =>
            x.Code == "ABC123" &&
            x.OriginalUrl == "https://google.com"
        )), Times.Once);
        
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShortenAsync_InvalidUrl_ThrowsArgumentException()
    {
        // Arrange
        var gemoMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        
        var service = new ShortenerService(gemoMock.Object, repoMock.Object, baseUrl);
        
        // Act
        Func<Task> act = () => service.ShortenAsync("");
        
        // Assert
        var ex = await act.Should().ThrowAsync<ArgumentException>();
        ex.Which.Message.Should().Contain("Invalid URL");
        ex.Which.ParamName.Should().Be("originalUrl");
    }

    [Fact]
    public async Task ShortenAsync_CodeCollision_RetriesAndSavesWithSecondCode()
    {
        // Arrange 
        var genMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        
        genMock.Setup(g => g.Generate(It.IsAny<int>())).Returns("AAA111");
        repoMock.SetupSequence(r => r.ExistsByCodeAsync("AAA111")).ReturnsAsync(true);
        
        genMock.Setup(g => g.Generate(It.IsAny<int>())).Returns("BBB222");
        repoMock.SetupSequence(r => r.ExistsByCodeAsync("BBB222")).ReturnsAsync(false);
        
        var service = new ShortenerService(genMock.Object, repoMock.Object, baseUrl);
        
        // Act 
        var result = await service.ShortenAsync("https://google.com");
        
        // Assert
        result.Code.Should().Be("BBB222");
        result.ShortUrl.Should().Be("http://localhost:5070/BBB222");

        repoMock.Verify(r => r.AddAsync(It.Is<ShortUrl>(x =>
            x.Code == "BBB222" &&
            x.OriginalUrl == "https://google.com"
        )), Times.Once);
        
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShortenAsync_AllAttemptsCollide_ThrowsException()
    {
        // Arrange
        var genMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        
        genMock
            .Setup(g => g.Generate(It.IsAny<int>()))
            .Returns("DUPLICATE");
        
        repoMock
            .Setup(r => r.ExistsByCodeAsync("DUPLICATE"))
            .ReturnsAsync(true);
        
        var service = new ShortenerService(genMock.Object, repoMock.Object, baseUrl);
        
        // Act
        var act = async () => await service.ShortenAsync("https://google.com");
        
        // Assert
        var ex = await Assert.ThrowsAsync<Exception>(act);
        ex.Message.Should().Be("Unable to generate unique code");
        
        repoMock.Verify(r => r.ExistsByCodeAsync("DUPLICATE"), Times.Exactly(11));
        
        repoMock.Verify(r => r.AddAsync(It.IsAny<UrlShortener.Core.Entities.ShortUrl>()), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ResolveOriginalUrlAndIncrementAsync_WhenExists_IncrementsClicksAndReturnsUrl()
    {
        // Arrange
        var genMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        var code = "ABC123";

        var entity = new ShortUrl()
        {
            Code = code,
            OriginalUrl = "https://google.com",
            Clicks = 5
        };

        repoMock
            .Setup(r => r.GetByCodeAsync(code))
            .ReturnsAsync(entity);
        
        var service = new ShortenerService(genMock.Object, repoMock.Object, baseUrl);
        
        // Act
        var result = await service.ResolveOriginalUrlAndIncrementAsync(code);
        
        // Assert
        result.Should().Be("https://google.com");

        repoMock.Verify(r => r.IncrementClicksAsync(code), Times.Once);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task ResolveOriginalUrlAndIncrementAsync_WhenNotFound_ReturnsNull_AndDoesNotSave()
    {
        // Arrange
        var genMock = new Mock<ICodeGenerator>();
        var repoMock = new Mock<IShortUrlRepository>();
        var baseUrl = "http://localhost:5070";
        var code = "ABC123";
        
        repoMock
            .Setup(r => r.GetByCodeAsync(code))
            .ReturnsAsync((ShortUrl?)null);

        var service = new ShortenerService(genMock.Object, repoMock.Object, baseUrl);
        
        // Act

        var result = await service.ResolveOriginalUrlAndIncrementAsync(code);
        
        // Assert
        result.Should().BeNull();

        repoMock.Verify(r => r.IncrementClicksAsync(code), Times.Never);
        repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}