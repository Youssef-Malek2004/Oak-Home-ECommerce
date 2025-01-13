var builder = DistributedApplication.CreateBuilder(args);

const string usingAspire = "true";
const string kafkaLocalConnectionString = "localhost:9092";
var useLocalKafka = false;

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .WithPgWeb();

var mongo = builder.AddMongoDB("mongo")
    .WithDataVolume();

var redis = builder.AddRedis("redis")
    .WithRedisInsight()
    .WithDataVolume();

var cartDatabase = postgres.AddDatabase("CartDatabase");
var inventoryDatabase = postgres.AddDatabase("InventoryDatabase");
var ordersDatabase = postgres.AddDatabase("OrdersDatabase");
var usersDatabase = postgres.AddDatabase("UsersDatabase");
var paymentsDatabase = postgres.AddDatabase("PaymentsDatabase");
var productsDatabase = mongo.AddDatabase("ProductsDatabase");

if (useLocalKafka)
{
    builder.AddProject<Projects.Users_Api>("api-service-users")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(usersDatabase)
        .WaitFor(usersDatabase)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Products_Api>("api-service-products")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(productsDatabase)
        .WaitFor(productsDatabase)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Inventory_Api>("api-service-inventory")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(inventoryDatabase)
        .WaitFor(inventoryDatabase)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Orders_Api>("api-service-orders")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(ordersDatabase)
        .WaitFor(ordersDatabase)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Notifications_Api>("api-service-notifications")
        .WithReference(redis)
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Payments_Api>("api-service-payments")
        .WithReference(paymentsDatabase)
        .WaitFor(paymentsDatabase)
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString);
    
    builder.AddProject<Projects.Cart_Api>("api-service-cart")
        .WithEnvironment("ConnectionStrings__kafka", kafkaLocalConnectionString)
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(cartDatabase)
        .WaitFor(cartDatabase);   
}
else
{
    var kafka = builder
        .AddKafka("kafka")
        .WithKafkaUI();
    
    builder.AddProject<Projects.Users_Api>("api-service-users")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(usersDatabase)
        .WaitFor(usersDatabase)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Products_Api>("api-service-products")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(productsDatabase)
        .WaitFor(productsDatabase)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Inventory_Api>("api-service-inventory")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(inventoryDatabase)
        .WaitFor(inventoryDatabase)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Orders_Api>("api-service-orders")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(ordersDatabase)
        .WaitFor(ordersDatabase)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Notifications_Api>("api-service-notifications")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(redis)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Payments_Api>("api-service-payments")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WithReference(paymentsDatabase)
        .WaitFor(paymentsDatabase)
        .WaitFor(kafka)
        .WithReference(kafka);

    builder.AddProject<Projects.Cart_Api>("api-service-cart")
        .WithEnvironment("Using__Aspire", usingAspire)
        .WaitFor(kafka)
        .WithReference(kafka)
        .WithReference(cartDatabase)
        .WaitFor(cartDatabase);   
}
    

builder.AddProject<Projects.Gateway_Api>("gateway-api")
    .WithEnvironment("Using__Aspire", usingAspire)
    .WithExternalHttpEndpoints();

builder.Build().Run();