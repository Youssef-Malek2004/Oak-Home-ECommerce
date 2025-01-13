var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendHttps", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
            .AllowCredentials() 
            .AllowAnyHeader()
            .AllowAnyMethod();
        
        policy.WithOrigins("http://localhost:5173")
            .AllowCredentials() 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    
    options.AddPolicy("AllowFrontendHttp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowCredentials() 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var usingAspire = Environment.GetEnvironmentVariable("Using__Aspire");

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (usingAspire is not null && usingAspire == "true")
{
    app.UseCors("AllowFrontendHttps");
}
else app.UseCors("AllowFrontendHttp");


app.UseHttpsRedirection();

app.UseWebSockets();
app.MapReverseProxy();

app.Run();