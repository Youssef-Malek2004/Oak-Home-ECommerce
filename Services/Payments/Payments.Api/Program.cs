using MediatR;
using Payments.Api.Extensions;
using Payments.Api.Middlewares;
using Payments.Api.OptionsSetup;
using Payments.Application.CQRS.Queries;
using Payments.Application.CQRS.QueryHandlers;
using Payments.Infrastructure;
using Payments.Infrastructure.Kafka;
using Shared.Contracts.Entities.NotificationService;
using Shared.Contracts.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Configuration["ConnectionStrings:DatabaseLocal"] = 
    Environment.GetEnvironmentVariable("DATABASE_LOCAL") ?? builder.Configuration["ConnectionStrings:DatabaseLocal"];
builder.Configuration["ConnectionStrings:DatabaseDocker"] = 
    Environment.GetEnvironmentVariable("DATABASE_DOCKER") ?? builder.Configuration["ConnectionStrings:DatabaseDocker"];
builder.Configuration["UsingDocker"] = 
    Environment.GetEnvironmentVariable("USING_DOCKER") ?? builder.Configuration["UsingDocker"];

var usingAspire = Environment.GetEnvironmentVariable("Using__Aspire");
if (usingAspire is not null && usingAspire == "true")
{
    builder.AddNpgsqlDataSource("PaymentsDatabase");
    builder.Services.AddAspirePersistence();
}
else builder.Services.AddPersistence(builder.Configuration);

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<GetUserPaymentsHandler>());

builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaNotificationService, KafkaNotificationsService>();
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

app.UseHttpsRedirection();
app.UseMiddleware<CookieToJwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VendorIdMiddleware>();

app.MapGet("Payments", async (IMediator mediator, HttpContext context) =>
{
    var result =await  mediator.Send(new GetUserPaymentsQuery(Guid.Parse((string)context.Items["VendorId"]!)));
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
}).RequireAuthorization();


app.AddLifetimeEvents();

app.Run();