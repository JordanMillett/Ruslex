using System.ComponentModel;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5148");

WebApplication app = builder.Build();

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
            
            var ru = parts[0];
            var en = parts[2]; // Assuming English translation is in the third column

            // Normalize the Russian word by removing apostrophes
            ru = ru.Replace("'", "");

            // Add the main form to translations dictionary
            if (!translations.ContainsKey(ru))
            {
                translations.Add(ru, new Translation(ru, en));
            }

            // Process all other forms and add them to the dictionary
            var forms = new List<string>
            {
                parts[10], // sg_nom
                parts[11], // sg_gen
                parts[12], // sg_dat
                parts[13], // sg_acc
                parts[14], // sg_inst
                parts[15], // sg_prep
                parts[16], // pl_nom
                parts[17], // pl_gen
                parts[18], // pl_dat
                parts[19], // pl_acc
                parts[20], // pl_inst
                parts[21]  // pl_prep
            };

            foreach (var form in forms)
            {
                var normalizedForm = form.Replace("'", "");
                if (!string.IsNullOrWhiteSpace(normalizedForm) && !translations.ContainsKey(normalizedForm))
                {
                    translations.Add(normalizedForm, new Translation(parts[0], en));
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
    Console.WriteLine($"Server crashed. Port 5148 may already be in use.");
}

public struct Translation
{
    public string RU { get; set; }
    public string EN { get; set; }

    public Translation(string ru, string en)
    {
        RU = ru;
        EN = en;
    }
}