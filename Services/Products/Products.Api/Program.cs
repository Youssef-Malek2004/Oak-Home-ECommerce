using Products.Api.Endpoints;
using Products.Api.Middlewares;
using Products.Api.OptionsSetup;
using Products.Application.Settings;
using Products.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddPersistence();
builder.Services.ConfigureMediatR();

var app = builder.Build();

app.InitializeDatabaseConnection(true, app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpoints = app.MapGroup("api");

endpoints.MapProductsCrudEndpoints();
endpoints.MapCategoryCrudEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();