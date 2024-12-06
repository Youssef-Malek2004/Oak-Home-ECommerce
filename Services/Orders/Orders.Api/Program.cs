using Orders.Api.Endpoints;
using Orders.Api.Extensions;
using Orders.Api.Middlewares;
using Orders.Api.OptionsSetup;
using Orders.Application.CQRS.CommandHandlers;
using Orders.Application.KafkaSettings;
using Orders.Application.Services.Kafka;
using Orders.Infrastructure;
using Orders.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<KafkaDispatcher>();
builder.Services.AddHostedService<KafkaHostedService>();

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommandHandler>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

var endpoints = app.MapGroup("api");

endpoints.MapOrdersCrudEndpoints();

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.AddLifetimeEvents();
app.UseHttpsRedirection();

app.Run();