using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Products.Domain.Entites;
[Collection("Products")]
public class Product : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] 
    public DateTime CreatedAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
    
}