using Shared.Contracts.Kafka;
using Users.Api.Endpoints;
using Users.Api.Extensions;
using Users.Api.Middlewares;
using Users.Api.OptionsSetup;
using Users.Infrastructure;
using Users.Infrastructure.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.ConfigureMediatR();

builder.Services.AddAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<KafkaEventProcessor>();
builder.Services.AddSingleton<KafkaDispatcher>();

builder.Services.AddHostedService<KafkaInitializationHostedService>();
builder.Services.AddHostedService<KafkaHostedService>();

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

app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.Run();