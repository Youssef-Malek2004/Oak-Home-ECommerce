using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace Products.Domain.Entities;

[Collection("Categories")]
public class Category : Entity
{
    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; } = null!;
    
}