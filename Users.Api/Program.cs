using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Api.Endpoints;
using Users.Api.Extensions;
using Users.Api.OptionsSetup;
using Users.Application.CQRS.Commands;
using Users.Application.Services;
using Users.Domain;
using Users.Domain.DTOs;
using Users.Domain.Entities;
using Users.Infrastructure;
using Users.Infrastructure.Authentication;
using Users.Infrastructure.CQRS.CommandHandlers;
using Users.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.ConfigureMediatR();

builder.Services.AddAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

var endpoints = app.MapGroup("api");

endpoints.MapGlobalEndpoints();
endpoints.MapCrudEndpoints();
endpoints.MapUserEndpoints();
endpoints.MapAdminEndpoints(); 


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();