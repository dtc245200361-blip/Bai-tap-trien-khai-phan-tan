
var builder = WebApplication.CreateBuilder(args);

// Dang ky dich vu
builder.Services.AddSingleton<GreetingService>();

var app = builder.Build();

// GET: Trang chu
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        application = "BasicWebApi",
        status = "running"
    });
});

// GET: Chao nguoi dung
app.MapGet("/hello/{name}", (string name, string? language) =>
{
    string message = language switch
    {
        "en" => $"Hello, {name}!",
        "ja" => $"Konnichiwa, {name}!",
        _ => $"Xin chao, {name}!"
    };

    return Results.Ok(new { message });
});

// POST: Tao loi chao
app.MapPost("/greetings", (CreateGreetingRequest request,
    GreetingService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new
        {
            message = "Name must not be empty."
        });
    }

    var greeting = service.Create(request.Name);

    return Results.Created(
        $"/greetings/{greeting.Id}",
        greeting
    );
});

// GET: Lay loi chao theo ID
app.MapGet("/greetings/{id:int}", (int id,
    GreetingService service) =>
{
    var greeting = service.GetById(id);

    return greeting is null
        ? Results.NotFound()
        : Results.Ok(greeting);
});

app.Run();

// Model du lieu
record CreateGreetingRequest(string Name);

record Greeting(int Id, string Name);

// Xu ly loi chao
class GreetingService
{
    private readonly List<Greeting> greetings = new();

    public Greeting Create(string name)
    {
        var greeting = new Greeting(
            greetings.Count + 1,
            name
        );

        greetings.Add(greeting);

        return greeting;
    }

    public Greeting? GetById(int id)
    {
        return greetings.FirstOrDefault(g => g.Id == id);
    }
}
