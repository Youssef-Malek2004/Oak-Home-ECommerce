using Microsoft.AspNetCore.Mvc;
using Products.Application.Services;
using Products.Application.Settings;
using Products.Domain.DTOs;
using Products.Infrastructure.Persistence;
using Products.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
builder.Services.AddSingleton<IProductsRepository, ProductsRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("test", async (IProductsRepository productsRepository) =>
{
    return Results.Ok(productsRepository.GetProducts().Result);
});

app.MapPost("products", async ([FromBody] CreateProductDto createProductDto, IProductsRepository productsRepository) =>
{
    var result = await productsRepository.CreateProduct(createProductDto);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
});

app.UseHttpsRedirection();

app.Run();