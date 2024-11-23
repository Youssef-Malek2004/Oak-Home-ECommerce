using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api.Extensions;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.DTOs;
using Users.Domain.Entities;
using Users.Domain.Mappers;
using Users.Domain.Repositories;
using Users.Infrastructure;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);

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

app.MapPost("/users", async ([FromBody]CreateUserDto createUserDto, IUserRepository userRepository, IUnitOfWork unitOfWork) =>
{
    var user = Mappers.CreateUserToUser(createUserDto);
    await userRepository.AddUserAsync(user);
    await unitOfWork.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
    
}).WithName("CreateUser");

app.MapGet("/users/{id:guid}", async (Guid id, IUnitOfWork unitOfWork) =>
{
    var user = await unitOfWork.UserRepository.GetUserByIdAsync(id);
    return user != null ? Results.Ok(user) : Results.NotFound();
});

app.MapGet("/users/{email}", async (string email, IUnitOfWork unitOfWork) =>
{
    var user = await unitOfWork.UserRepository.GetUserByEmailAsync(email);
    return user != null ? Results.Ok(user) : Results.NotFound();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();