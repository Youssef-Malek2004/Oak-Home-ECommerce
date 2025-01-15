# E-Commerce Microservices Platform

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

// ... other services configuration
```

#### ASP.NET Core & Entity Framework Core

- RESTful API implementation
- Entity Framework Core for ORM
- Code-first migrations
- Repository pattern implementation

Example service setup:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<AppDbContext>();
        services.AddKafka(Configuration);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => { /* JWT configuration */ });
    }
}
```

#### Apache Kafka

Event-driven architecture implementation:

```csharp
public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IProducer<string, string> _producer;

    public async Task HandleAsync(OrderCreatedEvent @event)
    {
        var message = new Message<string, string>
        {
            Key = @event.OrderId,
            Value = JsonSerializer.Serialize(@event)
        };

        await _producer.ProduceAsync("orders-topic", message);
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

#### Context API for State Management

```typescript
const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider: React.FC = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  const login = async (credentials: LoginCredentials) => {
    const response = await axios.post("/api/auth/login", credentials);
    setUser(response.data);
  };

  return <AuthContext.Provider value={{ user, login }}>{children}</AuthContext.Provider>;
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

This will start:

- All microservices
- PostgreSQL with PgAdmin
- MongoDB
- Redis with RedisInsight
- Kafka with UI
- API Gateway

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
├── AppHost/                 # Aspire orchestrator
├── Services/               # Backend microservices
│   ├── Users_Api/
│   ├── Products_Api/
│   ├── Inventory_Api/
│   ├── Orders_Api/
│   ├── Cart_Api/
│   ├── Payments_Api/
│   └── Notifications_Api/
├── Frontend/
│   └── EcommerceUI/       # React frontend
└── Gateway_Api/           # API Gateway
```

## 🔒 Security

- JWT Authentication
- HTTPS/SSL encryption
- Secure communication between services
- Environment-based configurations

## 🛠 Development

- Microservices architecture
- Event-driven communication
- CQRS pattern
- Real-time notifications
- Distributed caching

## 📝 License

[Your License Here]

## 👥 Contributors

[Your Contributors Here]

---

Made with ❤️ by [Your Team Name]
