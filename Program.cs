WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5147");

WebApplication app = builder.Build();

/*
Dictionary<string, Translation> translations = new Dictionary<string, Translation>
{
    { "привет", new Translation("hello", "greeting") },
    { "кошка", new Translation("cat", "noun") },
    { "собака", new Translation("dog", "noun") }
};
*/

Dictionary<string, Translation> translations = new Dictionary<string, Translation>();

// Load data from CSV NOUNS ONLY, FORMAT SPECIFIC TO NOUNS
string csvFilePath = Path.Combine("data", "nouns.csv");
if (File.Exists(csvFilePath))
{
    try
    {
        var lines = File.ReadAllLines(csvFilePath);
        foreach (var line in lines.Skip(1)) // Skip header line
        {
            var parts = line.Split('\t');
            if (parts.Length >= 3) // Ensure at least three parts: Russian word, English translation, and word type
            {
                var russianWord = parts[0];
                var englishTranslation = parts[2]; // Assuming English translation is in the third column
                var wordType = parts[5]; // Assuming word type is in the sixth column

                // Add to translations dictionary
                if (!translations.ContainsKey(russianWord))
                {
                    translations.Add(russianWord, new Translation(englishTranslation, wordType));
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading data from CSV: {ex.Message}");
    }
}
else
{
    Console.WriteLine($"CSV file '{csvFilePath}' not found.");
}

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

try
{
    app.Run();
}
catch
{
    Console.WriteLine($"Server crashed. Port 5147 may already be in use.");
}

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