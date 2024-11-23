var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/test", async (HttpContext context, HttpResponse response) =>
{
    await context.Response.WriteAsJsonAsync(1);
});
app.MapGet("/testing", async (HttpContext context, HttpResponse response) =>
{
    await context.Response.WriteAsJsonAsync(2);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();