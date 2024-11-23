using Microsoft.EntityFrameworkCore;
using Users.Api.Extensions;
using Users.Application.Services;
using Users.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IUsersDbContext, UsersDbContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.MapGet("/users", async (HttpContext context, UsersDbContext usersDbContext) =>
{
    var users = await usersDbContext.Users.ToListAsync();
    return Results.Ok(users);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();