# E-Commerce Microservices Platform

![.NET](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![React](https://img.shields.io/badge/React-61DAFB?style=for-the-badge&logo=react&logoColor=black)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white)
![Kafka](https://img.shields.io/badge/Apache%20Kafka-231F20?style=for-the-badge&logo=apache-kafka&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

A modern, cloud-native e-commerce platform built with .NET 8 Aspire and React, implementing a microservices architecture for scalability and maintainability.

## 🏗 Architecture Overview

### Backend Services

- **Users Service** - Authentication and user management
- **Products Service** - Product catalog and management
- **Inventory Service** - Stock management
- **Orders Service** - Order processing and management
- **Cart Service** - Shopping cart operations
- **Payments Service** - Payment processing
- **Notifications Service** - Real-time notifications
- **API Gateway** - Central gateway for all services

### Data Stores

- **PostgreSQL Databases**
  - Cart Database
  - Inventory Database
  - Orders Database
  - Users Database
  - Payments Database
- **MongoDB** - Products catalog
- **Redis** - Caching and real-time features
- **Apache Kafka** - Event streaming and messaging

## 🛠 Tech Stack

### Backend

#### .NET 8 Aspire

Cloud-native application platform for orchestrating microservices. Here's how we use Aspire to configure our services:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Database Configuration
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var mongo = builder.AddMongoDB("mongo")
    .WithDataVolume();

var redis = builder.AddRedis("redis")
    .WithRedisInsight();

// Service Configuration
builder.AddProject<Projects.Users_Api>("api-service-users")
    .WithReference(usersDatabase)
    .WithReference(kafka);

builder.AddProject<Projects.Products_Api>("api-service-products")
    .WithReference(productsDatabase)
    .WithReference(kafka);
```

#### Microservice Setup (Inventory Service Example)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Service Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
var usingAspire = Environment.GetEnvironmentVariable("Using__Aspire");
if (usingAspire is not null && usingAspire == "true")
{
    builder.AddNpgsqlDataSource("InventoryDatabase");
    builder.Services.AddAspirePersistence();
}
else builder.Services.AddPersistence(builder.Configuration);

// Kafka Configuration
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddKafkaAdminClient();
builder.Services.AddSingleton<IKafkaProducerService,KafkaProducerService>();
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();

// Authentication & Authorization
builder.Services.ConfigureAuthenticationAndAuthorization();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();
```

#### Apache Kafka Implementation

Real implementation of Kafka producer service:

```csharp
public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IOptions<KafkaSettings> settings)
    {
        var kafkaConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__kafka");

        var config = new ProducerConfig
        {
            BootstrapServers = kafkaConnectionString ?? settings.Value.BootstrapServers,
            AllowAutoCreateTopics = true,
            BatchSize = 16384,
            LingerMs = 5,
            Acks = Acks.All,
            SecurityProtocol = SecurityProtocol.Plaintext
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendMessageAsync<T>(string topic, T message, CancellationToken cancellationToken, string eventType)
    {
        var serializedMessage = JsonSerializer.Serialize(message);

        try
        {
            var result = await _producer.ProduceAsync(topic,
                new Message<string, string> {
                    Key= Guid.NewGuid().ToString(),
                    Value = serializedMessage,
                    Headers = new Headers {
                        { "eventType", Encoding.UTF8.GetBytes(eventType) }
                    }
                }, cancellationToken);
            Console.WriteLine($"Message sent to topic {topic}: {result.Value} Offset: {result.Offset}");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Error producing message: {ex.Error.Reason}");
        }

        _producer.Flush(cancellationToken);
    }
}
```

#### MongoDB Integration

Product service document model:

```csharp
public class Product
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Images { get; set; }
}
```

### Frontend

#### React 18 with TypeScript

Modern frontend implementation with type safety:

```typescript
interface Product {
  id: string;
  name: string;
  price: number;
  description: string;
  categories: string[];
  images: string[];
}

const ProductList: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    const fetchProducts = async () => {
      const response = await axios.get("/api/products");
      setProducts(response.data);
    };
    fetchProducts();
  }, []);

  return (
    <Grid container spacing={2}>
      {products.map((product) => (
        <ProductCard key={product.id} product={product} />
      ))}
    </Grid>
  );
};
```

#### Material-UI Components

Responsive UI implementation:

```typescript
const ProductCard: React.FC<{ product: Product }> = ({ product }) => {
  return (
    <Card>
      <CardMedia component="img" height="200" image={product.images[0]} alt={product.name} />
      <CardContent>
        <Typography variant="h6">{product.name}</Typography>
        <Typography variant="body2">{product.description}</Typography>
        <Typography variant="h6">${product.price}</Typography>
      </CardContent>
      <CardActions>
        <Button variant="contained" color="primary">
          Add to Cart
        </Button>
      </CardActions>
    </Card>
  );
};
```

### Development Tools

#### Docker Containerization

Example Docker Compose configuration:

```yaml
version: "3.8"
services:
  postgres:
    image: postgres:latest
    environment:
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    volumes:
      - postgres_data:/var/lib/postgresql/data

  mongodb:
    image: mongo:latest
    volumes:
      - mongo_data:/data/db

  redis:
    image: redis:latest
    ports:
      - "6379:6379"

  kafka:
    image: confluentinc/cp-kafka:latest
    environment:
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9092
```

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js (v18+)
- Docker Desktop
- Git

### Backend Setup

1. Clone the repository:

```bash
git clone [repository-url]
```

2. Navigate to the AppHost directory:

```bash
cd AppHost
```

3. Run the Aspire orchestrator:

```bash
dotnet run
```

### Frontend Setup

1. Navigate to the frontend directory:

```bash
cd Frontend/EcommerceUI
```

2. Install dependencies:

```bash
npm install
```

3. Start the development server:

```bash
npm run dev
```

## 📁 Project Structure

```
├── AppHost/                    # Aspire orchestrator
│
├── Services/                   # Microservices
│   ├── Users_Api/             # User authentication & management
│   ├── Products_Api/          # Product catalog management
│   ├── Inventory_Api/         # Stock management
│   ├── Orders_Api/            # Order processing
│   ├── Cart_Api/              # Shopping cart
│   ├── Payments_Api/          # Payment processing
│   └── Notifications_Api/     # Real-time notifications
│
├── Gateway_Api/               # API Gateway & routing
│
├── Frontend/
│   └── EcommerceUI/          # React frontend
│       ├── src/
│       │   ├── Components/   # UI components
│       │   ├── Pages/        # Page components
│       │   ├── Services/     # API services
│       │   └── Assets/       # Static assets
│       └── public/           # Public assets
│
├── Shared/                    # Shared code
│   └── Shared.Contracts/     # DTOs & interfaces
│
└── docker-compose.yml         # Docker configuration
```

Each microservice is structured using clean architecture:

- Domain models
- Business logic
- Data access
- API endpoints

---

Made with ❤️ using .NET 8 Aspire & React
