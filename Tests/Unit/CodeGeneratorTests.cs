using FluentAssertions;
using UrlShortener.Core.Services;
using Xunit;

namespace UrlShortener.Tests.Unit;

public class CodeGeneratorTests
{
    [Fact]
    public void Generate_DefaultLength_Returns6Characters()
    {
        // Arrange 
        var gen = new CodeGenerator();

        // Act
        var code = gen.Generate();
        
        // Assert
        code.Should().NotBeNullOrWhiteSpace();
        code.Length.Should().Be(6);
    }

    [Fact]
    public void Generate_CustomLength_ReturnsCorrectLength()
    {
        // Arrange
        var gen = new CodeGenerator();
        
        // Act 
        var code = gen.Generate(10);
        
        // Assert
        code.Should().NotBeNullOrWhiteSpace();
        code.Length.Should().Be(10);
    }

    [Fact]
    public void Generate_OnlyAllowedChars()
    {
        // Arrange
        var gen = new CodeGenerator();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Act 
        var code = gen.Generate();
        
        // Assert
        foreach (var c in code)
        {
            chars.Contains(c).Should().BeTrue();
        }
    }
}