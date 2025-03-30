
using EarleyParser;

var builder = WebApplication.CreateBuilder(args);

// Register CORS services before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer(); // Required for Minimal APIs
builder.Services.AddSwaggerGen(); // Registers Swagger

var app = builder.Build();

app.UseHttpsRedirection(); 
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(); //testing.


var grammarsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Grammars");
var vocsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Vocs");


// Ensure directory exists
if (!Directory.Exists(grammarsDirectory))
{
    Directory.CreateDirectory(grammarsDirectory);
}
if (!Directory.Exists(vocsDirectory))
{
    Directory.CreateDirectory(vocsDirectory);
}


app.MapGet("/", () => "Hello Earley Parser!");


app.MapGet("/grammars", () =>
{
    var files = Directory.GetFiles(grammarsDirectory)
                         .Select(Path.GetFileName)
                         .ToList();
    return Results.Ok(files);
});


// GET /grammars/{filename} - Serve a specific file
app.MapGet("/grammars/{filename}", (string filename) =>
{
    var filePath = Path.Combine(grammarsDirectory, filename);

    if (!System.IO.File.Exists(filePath))
    {
        return Results.NotFound("File not found");
    }

    return Results.File(filePath, "text/plain"); // Change MIME type as needed
});


app.MapGet("/vocs", () =>
{
    var files = Directory.GetFiles(vocsDirectory)
                         .Select(Path.GetFileName)
                         .ToList();
    return Results.Ok(files);
});


// GET /vocs/{filename} - Serve a specific file
app.MapGet("/vocs/{filename}", (string filename) =>
{
    var filePath = Path.Combine(vocsDirectory, filename);

    if (!System.IO.File.Exists(filePath))
    {
        return Results.NotFound("File not found");
    }

    return Results.File(filePath, "text/plain"); // Change MIME type as needed
});

app.MapPost("/ParseSentence",  (EarleyParserParams p) =>
{
    var POSRules = GrammarFileReader.ReadRules(p.PartOfSpeechRules);
    var voc = new Vocabulary(POSRules);
    var grammarRules = GrammarFileReader.ReadRules(p.GrammarRules);
    ContextFreeGrammar g = new ContextFreeGrammar(grammarRules);
    var text = p.Sentence.Split();
    var parser = new EarleyParser.EarleyParser(g, voc, text);
    (var accepted, var res) = parser.ParseSentence();
    return Results.Json(res);

});

app.Run();
