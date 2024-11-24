using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api.Extensions;
using Users.Api.OptionsSetup;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.DTOs;
using Users.Domain.Mappers;
using Users.Domain.Repositories;
using Users.Infrastructure;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.CQRS.CommandHandlers;
using Users.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.MapPost("/signup", async ([FromBody] SignUpDto signUpDto ,IMediator mediator) =>
{
    var result = await mediator.Send(new SignUpCommand(signUpDto));
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapPost("/login", async ([FromBody] LoginDto loginDto, IMediator mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new LoginCommand(loginDto), cancellationToken);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.MapGet("/users", async (HttpContext context, UsersDbContext usersDbContext) =>
{
    var users = await usersDbContext.Users.ToListAsync();
    return Results.Ok(users);
}).RequireAuthorization();

app.MapGet("/users/{id:guid}", async (Guid id, IUnitOfWork unitOfWork) =>
{
    var result = await unitOfWork.UserRepository.GetUserByIdAsync(id);

    if (result.IsFailure)
    {
        return Results.NotFound(new
        {
            Code = result.Error.Code,
            Message = result.Error.Description
        });
    }

    return Results.Ok(result.Value);
});

app.MapGet("/users/{email}", async (string email, IUnitOfWork unitOfWork) =>
{
    var result = await unitOfWork.UserRepository.GetUserByEmailAsync(email);

    if (result.IsFailure)
    {
        return Results.NotFound(new
        {
            Code = result.Error.Code,
            Message = result.Error.Description
        });
    }

    return Results.Ok(result.Value);
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();