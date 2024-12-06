using Orders.Api.Endpoints;
using Orders.Api.Extensions;
using Orders.Application.CQRS.CommandHandlers;
using Orders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommandHandler>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapOrdersCrudEndpoints();

app.UseHttpsRedirection();

app.Run();