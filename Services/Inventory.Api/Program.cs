var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("test", async (HttpContext context, HttpRequest request) =>
{
    await context.Response.WriteAsJsonAsync("success");
});

app.UseHttpsRedirection();

app.Run();