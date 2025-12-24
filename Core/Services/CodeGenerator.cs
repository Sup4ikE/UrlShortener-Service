using UrlShortener.Core.Abstractions;

namespace UrlShortener.Core.Services;

public class CodeGenerator: ICodeGenerator
{
    public string Generate(int length = 6)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }
}