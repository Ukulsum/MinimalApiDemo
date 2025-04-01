using Microsoft.EntityFrameworkCore;
using MinimalApiDemo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    //options.UseInMemoryDatabase("TestDb");
    options.UseSqlServer(builder.Configuration.GetConnectionString("MinimalDbConnection"));
});
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

app.MapGet("api/posts", (ApplicationDbContext db) =>
{
    var post = db.Posts.ToList();
    return Results.Ok(post);
});

app.MapPost("api/posts", (Post post, ApplicationDbContext db) =>
{
    db.Posts.Add(post);
    bool isSaved = db.SaveChanges()>0;
    if (isSaved)
    {
        return Results.Ok("Post has been saved.");
    }
    return Results.BadRequest("Post saved failed.");
});

app.MapPut("api/posts", (int id, Post post, ApplicationDbContext db) =>
{
    var data = db.Posts.FirstOrDefault(x => x.Id == id);
    if(data == null)
    {
        return Results.NotFound();
    }
    if(data.Id != post.Id)
    {
        return Results.BadRequest("Id not valid.");
    }

    data.Title = post.Title;
    data.Description = post.Description;
    bool isUpdated = db.SaveChanges() > 0;
    if (isUpdated)
    {
        return Results.Ok("Post has been modified.");
    }
    return Results.BadRequest("Post modified failed.");
});


app.MapDelete("api/posts", (int id, ApplicationDbContext db) =>
{
    var post = db.Posts.FirstOrDefault(s => s.Id == id);
    if (post == null)
    {
        return Results.NotFound();
    }
    db.Posts.Remove(post);
    if(db.SaveChanges() > 0)
    {
        return Results.Ok("Post has been deleted.");
    }
    return Results.BadRequest("post Delete failed.");
});



app.Run();