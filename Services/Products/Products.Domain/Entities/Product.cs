using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using Products.Domain.Entities.Products;

namespace Products.Domain.Entities;
[Collection("Products")]
[BsonDiscriminator(RootClass = true)]
[BsonKnownTypes(typeof(ElectronicsProduct), typeof(ClothingProduct),
    typeof(BookProduct), typeof(HomeAppliancesProduct), typeof(ToyProduct),
    typeof(SportsProduct), typeof(BeautyProduct), typeof(HealthProduct),
    typeof(AutomotiveProduct), typeof(FoodAndBeverageProduct))]
public class Product : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] 
    public DateTime CreatedAt { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = [];
    public List<string> Tags { get; set; } = [];
    public string Sku { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; } = false; 
    public bool Featured { get; set; } = false;  

}