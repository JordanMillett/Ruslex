using System.ComponentModel;
using System.Diagnostics;

public class Program
{
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
    
    public static Dictionary<string, Translation> translations = new Dictionary<string, Translation>();
        
    public static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
            
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.WebHost.UseUrls("http://localhost:5148");

        WebApplication app = builder.Build();

        LoadNouns();
        LoadAdjectives();
        LoadVerbs();
        LoadOther();
        
        app.MapGet("/", (HttpContext context) =>
        {
            return Results.Ok(true);
        });
        
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
        
        stopwatch.Stop(); 
        TimeSpan elapsed = stopwatch.Elapsed;
        
        try
        {
            Console.WriteLine($"{translations.Count} translations loaded in {elapsed.TotalSeconds} seconds");
            app.Run();
        }
        catch
        {
            Console.WriteLine($"Server crashed. Port 5148 may already be in use.");
        }
        
    }
        
    // Load data from CSV NOUNS ONLY, FORMAT SPECIFIC TO NOUNS
    public static void LoadNouns()
    {
        string csvFilePath = Path.Combine("data", "nouns.csv");
        
        var lines = File.ReadAllLines(csvFilePath);
        foreach (var line in lines.Skip(1)) // Skip header line
        {
            var parts = line.Split('\t');
            
            var ru = parts[0];
            var en = parts[2]; // Assuming English translation is in the third column

            // Normalize the Russian word by removing apostrophes
            ru = ru.Replace("'", "").Replace("ё", "е");

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
                var normalizedForm = form.Replace("'", "").Replace("ё", "е");
                if (!string.IsNullOrWhiteSpace(normalizedForm) && !translations.ContainsKey(normalizedForm))
                {
                    translations.Add(normalizedForm, new Translation(parts[0], en));
                }
            }
                
        }
    }
    
    public static void LoadAdjectives()
    {
        string csvFilePath = Path.Combine("data", "adjectives.csv");
        
        var lines = File.ReadAllLines(csvFilePath);
        foreach (var line in lines.Skip(1)) // Skip header line
        {
            var parts = line.Split('\t');
            
            var ru = parts[0];
            var en = parts[2]; // Assuming English translation is in the third column

            // Normalize the Russian word by removing apostrophes
            ru = ru.Replace("'", "").Replace("ё", "е");

            // Add the main form to translations dictionary
            if (!translations.ContainsKey(ru))
            {
                translations.Add(ru, new Translation(ru, en));
            }
            
            // Process all other forms and add them to the dictionary
            var forms = new List<string>
            {
                parts[4],  // comparative
                parts[5],  // superlative
                parts[6],  // short_m
                parts[7],  // short_f
                parts[8],  // short_n
                parts[9],  // short_pl
                parts[10], // decl_m_nom
                parts[11], // decl_m_gen
                parts[12], // decl_m_dat
                parts[13], // decl_m_acc
                parts[14], // decl_m_inst
                parts[15], // decl_m_prep
                parts[16], // decl_f_nom
                parts[17], // decl_f_gen
                parts[18], // decl_f_dat
                parts[19], // decl_f_acc
                parts[20], // decl_f_inst
                parts[21], // decl_f_prep
                parts[22], // decl_n_nom
                parts[23], // decl_n_gen
                parts[24], // decl_n_dat
                parts[25], // decl_n_acc
                parts[26], // decl_n_inst
                parts[27], // decl_n_prep
                parts[28], // decl_pl_nom
                parts[29], // decl_pl_gen
                parts[30], // decl_pl_dat
                parts[31], // decl_pl_acc
                parts[32], // decl_pl_inst
                parts[33]  // decl_pl_prep
            };

            foreach (var form in forms)
            {
                var normalizedForm = form.Replace("'", "").Replace("ё", "е");
                if (!string.IsNullOrWhiteSpace(normalizedForm) && !translations.ContainsKey(normalizedForm))
                {
                    translations.Add(normalizedForm, new Translation(parts[0], en));
                }
            }
                
        }
    }
    
    public static void LoadVerbs()
    {
        string csvFilePath = Path.Combine("data", "verbs.csv");
        
        var lines = File.ReadAllLines(csvFilePath);
        foreach (var line in lines.Skip(1)) // Skip header line
        {
            var parts = line.Split('\t');
            
            var ru = parts[0];
            var en = parts[2]; // Assuming English translation is in the third column

            // Normalize the Russian word by removing apostrophes
            ru = ru.Replace("'", "").Replace("ё", "е");

            // Add the main form to translations dictionary
            if (!translations.ContainsKey(ru))
            {
                translations.Add(ru, new Translation(ru, en));
            }
            
            // Process all other forms and add them to the dictionary
            var forms = new List<string>
            {
                parts[6],  // imperative_sg
                parts[7],  // imperative_pl
                parts[8],  // past_m
                parts[9],  // past_f
                parts[10], // past_n
                parts[11], // past_pl
                parts[12], // presfut_sg1
                parts[13], // presfut_sg2
                parts[14], // presfut_sg3
                parts[15], // presfut_pl1
                parts[16], // presfut_pl2
                parts[17]  // presfut_pl3
            };

            foreach (var form in forms)
            {
                var normalizedForm = form.Replace("'", "").Replace("ё", "е");
                if (!string.IsNullOrWhiteSpace(normalizedForm) && !translations.ContainsKey(normalizedForm))
                {
                    translations.Add(normalizedForm, new Translation(parts[0], en));
                }
            }
                
        }
    }

    public static void LoadOther()
    {
        string csvFilePath = Path.Combine("data", "others.csv");
        
        var lines = File.ReadAllLines(csvFilePath);
        foreach (var line in lines.Skip(1)) // Skip header line
        {
            var parts = line.Split('\t');
            
            var ru = parts[0];
            var en = parts[2]; // Assuming English translation is in the third column

            // Add the main form to translations dictionary
            if (!translations.ContainsKey(ru))
            {
                translations.Add(ru, new Translation(ru, en));
            }
        }
    }
}