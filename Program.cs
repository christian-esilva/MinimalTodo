using Microsoft.EntityFrameworkCore;
using MinimalTodo.Data;
using MinimalTodo.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("v1/todos", async (AppDbContext context) =>
{
    var todos = context.Todos.ToListAsync();
    return Results.Ok(todos);
}).Produces<Todo>();

app.MapGet("v1/todos/{id:guid}", async (AppDbContext context, Guid id) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
    if(todo == null) return Results.NotFound();

    return Results.Ok(todo);
}).Produces<Todo>();

app.MapPost("v1/todos", async (AppDbContext context, CreateTodoViewModel model) =>
{
    var todo = model.MapTo();
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.Todos.Add(todo);
    await context.SaveChangesAsync();

    return Results.Created($"/v1/todos{todo.Id}", todo);
}).Produces<Todo>();

app.MapPut("v1/todos/{id:guid}", async (AppDbContext context, Guid id, UpdateTodoViewModel model) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
    if (todo == null) return Results.BadRequest();

    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    todo = model.MapTo(id);

    context.ChangeTracker.Clear();
    context.Todos.Update(todo);
    await context.SaveChangesAsync();

    return Results.Ok(todo);
}).Produces<Todo>();

app.MapDelete("v1/todos/{id:guid}", async (AppDbContext context, Guid id) =>
{
    var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);
    if (todo == null) return Results.BadRequest();

    context.Todos.Remove(todo);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
