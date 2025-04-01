using MinimalApiDemo;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () =>
{
    return "Hello World";
});

app.MapPost("/", () =>
{
    return Results.Ok("Call Post action method.");
});

app.MapPut("/", () =>
{
    return Results.Ok("Call put action method.");
});

app.MapDelete("/", () =>
{
    return Results.Ok("Call delete action method");
});

// Bind Parameter
app.MapPost("Parameter", (string name) =>
{
    return Results.Ok("Hello" + name);
});

app.MapPost("post", (Post post) =>
{
    return Results.Ok(post);
});
app.Run();