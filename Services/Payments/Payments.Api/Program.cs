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

builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddPersistence(builder.Configuration);
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
    var vendorId = (string)context.Items["VendorId"]!;
    var result =await  mediator.Send(new GetUserPaymentsQuery(Guid.Parse((string)context.Items["VendorId"]!)));
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
}).RequireAuthorization();



app.AddLifetimeEvents();

app.Run();