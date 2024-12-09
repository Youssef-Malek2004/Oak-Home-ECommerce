using Products.Api.Endpoints;
using Products.Api.Middlewares;
using Products.Api.OptionsSetup;
using Products.Application.Settings;
using Products.Infrastructure;
using Products.Infrastructure.Kafka;
using Shared.Contracts.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddPersistence();
builder.Services.ConfigureMediatR();

builder.Services.AddKafkaAdminClient();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddSingleton<KafkaConsumerService>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddHostedService<KafkaHostedService>();

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

app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("Application started.");
});

app.Lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("Application stopped.");
});

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.Run();