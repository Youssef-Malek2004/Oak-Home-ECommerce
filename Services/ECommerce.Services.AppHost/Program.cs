var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Users_Api>("api-service-users").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Products_Api>("api-service-products").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Inventory_Api>("api-service-inventory").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Orders_Api>("api-service-orders").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Notifications_Api>("api-service-notifications").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
builder.AddProject<Projects.Payments_Api>("api-service-payments").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");

builder.AddProject<Projects.Gateway_Api>("gateway-api");
builder.AddProject<Projects.AppTestUI>("app-testing-ui");

builder.Build().Run();