var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Users_Api>("api-service-users").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Products_Api>("api-service-products").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Inventory_Api>("api-service-inventory").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Orders_Api>("api-service-orders").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");

builder.Build().Run();