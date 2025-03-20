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


var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/", () => "Hello Earley Parser!");

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
