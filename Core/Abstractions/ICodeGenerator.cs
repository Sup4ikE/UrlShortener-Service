namespace UrlShortener.Core.Abstractions;

public interface ICodeGenerator
{
    string Generate(int length = 6);
}