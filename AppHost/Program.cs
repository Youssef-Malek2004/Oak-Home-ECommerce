var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin().WithPgWeb();
var cartDatabase = postgres.AddDatabase("CartDatabase");

var kafka = builder.AddKafka("kafka").WithKafkaUI();

// builder.AddProject<Projects.Users_Api>("api-service-users").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
// builder.AddProject<Projects.Products_Api>("api-service-products").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
// builder.AddProject<Projects.Inventory_Api>("api-service-inventory").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
// builder.AddProject<Projects.Orders_Api>("api-service-orders").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
// builder.AddProject<Projects.Notifications_Api>("api-service-notifications").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");
// builder.AddProject<Projects.Payments_Api>("api-service-payments").WithEnvironment("ConnectionStrings__kafka", "localhost:9092");

builder.AddProject<Projects.Users_Api>("api-service-users")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Products_Api>("api-service-products")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Inventory_Api>("api-service-inventory")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Orders_Api>("api-service-orders")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Notifications_Api>("api-service-notifications")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Payments_Api>("api-service-payments")
    .WaitFor(kafka)
    .WithReference(kafka);

builder.AddProject<Projects.Cart_Api>("api-service-cart")
    .WaitFor(kafka)
    .WithReference(kafka)
    .WithReference(cartDatabase)
    .WaitFor(cartDatabase);
    

builder.AddProject<Projects.Gateway_Api>("gateway-api");

builder.Build().Run();