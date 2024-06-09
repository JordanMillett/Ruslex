using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5147");

WebApplication app = builder.Build();

Dictionary<string, Translation> translations = new Dictionary<string, Translation>
{
    { "привет", new Translation("hello", "greeting") },
    { "кошка", new Translation("cat", "noun") },
    { "собака", new Translation("dog", "noun") }
};

app.MapGet("/translations", (HttpContext context) =>
{
    return Results.Ok(translations);
});

app.MapGet("/translations/{russianTerm}", (HttpContext context, string russianTerm) =>
{
    if (translations.TryGetValue(russianTerm, out Translation translation))
    {
        return Results.Ok(translation);
    }
    return Results.NotFound();
});

app.Run();

public struct Translation
{
    public string EnglishTranslation { get; set; }
    public string WordType { get; set; }

    public Translation(string englishTranslation, string wordType)
    {
        EnglishTranslation = englishTranslation;
        WordType = wordType;
    }
}