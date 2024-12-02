using MongoDB.Entities;
using Products.Domain.Entities;
using Products.Domain.Entities.Products;

namespace Products.Infrastructure.Persistence;

public static class Seedings
{
    public static async Task SeedCategoriesAsync()
    {
        var existingCategories = await DB.Find<Category>().ExecuteAsync();

        if (!existingCategories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing" },
                new Category { Name = "Books" },
                new Category { Name = "HomeAppliances" },
                new Category { Name = "Toys" },
                new Category { Name = "Sports" },
                new Category { Name = "Health" },
                new Category { Name = "Beauty" },
                new Category { Name = "Automotive" },
                new Category { Name = "FoodAndBeverages" }
            };

            await DB.SaveAsync(categories);
            Console.WriteLine("Categories seeded successfully.");
        }
        else
        {
            Console.WriteLine("Categories already exist. No seeding required.");
        }
    }

    public static async Task SeedProductsAsync()
    {
        var existingProducts = await DB.Find<Product>().ExecuteAsync();

        if (!existingProducts.Any())
        {
            var categories = await DB.Find<Category>().ExecuteAsync();

            var products = new List<Product>();

            foreach (var category in categories)
            {
                switch (category.Name)
                {
                    case "Electronics":
                        products.AddRange(new[]
                        {
                            new ElectronicsProduct
                            {
                                Name = "Samsung TV",
                                Description = "High-quality smart TV",
                                CategoryId = category.ID,
                                Price = 999.99M,
                                VendorId = "vendor1",
                                Sku = "ELEC-TV-001",
                                Tags = ["smart-tv", "electronics"],
                                Brand = "Samsung",
                                Model = "QLED2024",
                                WarrantyPeriod = 24
                            },
                            new ElectronicsProduct
                            {
                                Name = "Apple iPhone",
                                Description = "Latest model smartphone",
                                CategoryId = category.ID,
                                Price = 1099.99M,
                                VendorId = "vendor2",
                                Sku = "ELEC-PHONE-002",
                                Tags = ["smartphone", "electronics"],
                                Brand = "Apple",
                                Model = "iPhone 15",
                                WarrantyPeriod = 12
                            }
                        });
                        break;

                    case "Clothing":
                        products.AddRange(new[]
                        {
                            new ClothingProduct
                            {
                                Name = "Men's T-Shirt",
                                Description = "Comfortable cotton T-shirt",
                                CategoryId = category.ID,
                                Price = 19.99M,
                                VendorId = "vendor3",
                                Sku = "CLOTH-TSHIRT-001",
                                Tags = ["clothing", "men", "t-shirt"],
                                Size = "M",
                                Material = "100% Cotton",
                                Gender = "Men"
                            },
                            new ClothingProduct
                            {
                                Name = "Women's Jeans",
                                Description = "Stylish denim jeans",
                                CategoryId = category.ID,
                                Price = 49.99M,
                                VendorId = "vendor4",
                                Sku = "CLOTH-JEANS-002",
                                Tags = ["clothing", "women", "jeans"],
                                Size = "L",
                                Material = "Denim",
                                Gender = "Women"
                            }
                        });
                        break;

                    case "Books":
                        products.AddRange(new[]
                        {
                            new BookProduct
                            {
                                Name = "Effective C#",
                                Description = "50 ways to improve your C# programs",
                                CategoryId = category.ID,
                                Price = 39.99M,
                                VendorId = "vendor5",
                                Sku = "BOOK-C#-001",
                                Tags = ["books", "programming", "C#"],
                                Author = "Bill Wagner",
                                Publisher = "Addison-Wesley",
                                Pages = 350,
                                Isbn = "9780135159941"
                            },
                            new BookProduct
                            {
                                Name = "Clean Code",
                                Description = "A handbook of agile software craftsmanship",
                                CategoryId = category.ID,
                                Price = 49.99M,
                                VendorId = "vendor6",
                                Sku = "BOOK-CLEAN-002",
                                Tags = ["books", "programming", "clean-code"],
                                Author = "Robert C. Martin",
                                Publisher = "Prentice Hall",
                                Pages = 464,
                                Isbn = "9780132350884"
                            }
                        });
                        break;

                    case "HomeAppliances":
                        products.AddRange(new[]
                        {
                            new HomeAppliancesProduct
                            {
                                Name = "Energy-Efficient Refrigerator",
                                Description = "Advanced cooling technology",
                                CategoryId = category.ID,
                                Price = 799.99M,
                                VendorId = "vendor7",
                                Sku = "HOME-FRIDGE-001",
                                Tags = ["home-appliances", "refrigerator"],
                                Brand = "LG",
                                PowerConsumption = 150,
                                IsEnergyEfficient = true
                            },
                            new HomeAppliancesProduct
                            {
                                Name = "Microwave Oven",
                                Description = "Compact microwave for fast cooking",
                                CategoryId = category.ID,
                                Price = 199.99M,
                                VendorId = "vendor8",
                                Sku = "HOME-MICRO-002",
                                Tags = ["home-appliances", "microwave"],
                                Brand = "Panasonic",
                                PowerConsumption = 1200,
                                IsEnergyEfficient = false
                            }
                        });
                        break;

                    // Add cases for "Toys", "Sports", "Health", "Beauty", "Automotive", "FoodAndBeverages" here...

                    case "Toys":
                    products.AddRange(new[]
                    {
                        new ToyProduct
                        {
                            Name = "Lego Building Set",
                            Description = "Creative Lego set for kids aged 6 and above",
                            CategoryId = category.ID,
                            Price = 49.99M,
                            VendorId = "vendor9",
                            Sku = "TOYS-LEGO-001",
                            Tags = ["toys", "lego", "building"],
                            MinimumAge = 6,
                            Material = "Plastic",
                            IsEducational = true
                        },
                        new ToyProduct
                        {
                            Name = "Toy Car",
                            Description = "Remote-controlled toy car",
                            CategoryId = category.ID,
                            Price = 29.99M,
                            VendorId = "vendor10",
                            Sku = "TOYS-CAR-002",
                            Tags = ["toys", "car", "remote-control"],
                            MinimumAge = 8,
                            Material = "Metal and Plastic",
                            IsEducational = false
                        }
                    });
                    break;

                case "Sports":
                    products.AddRange(new[]
                    {
                        new SportsProduct
                        {
                            Name = "Basketball",
                            Description = "High-quality basketball suitable for indoor and outdoor use",
                            CategoryId = category.ID,
                            Price = 29.99M,
                            VendorId = "vendor11",
                            Sku = "SPORTS-BALL-001",
                            Tags = ["sports", "basketball"],
                            SportType = "Basketball",
                            Brand = "Spalding",
                            Size = "7"
                        },
                        new SportsProduct
                        {
                            Name = "Tennis Racket",
                            Description = "Professional-grade tennis racket",
                            CategoryId = category.ID,
                            Price = 99.99M,
                            VendorId = "vendor12",
                            Sku = "SPORTS-RACKET-002",
                            Tags = ["sports", "tennis"],
                            SportType = "Tennis",
                            Brand = "Wilson",
                            Size = "Standard"
                        }
                    });
                    break;

                case "Health":
                    products.AddRange(new[]
                    {
                        new HealthProduct
                        {
                            Name = "Vitamin C Tablets",
                            Description = "Boost your immunity with Vitamin C",
                            CategoryId = category.ID,
                            Price = 14.99M,
                            VendorId = "vendor13",
                            Sku = "HEALTH-VITC-001",
                            Tags = ["health", "vitamins"],
                            Ingredients = "Vitamin C, Zinc",
                            IsOrganic = true,
                            UsageInstructions = "Take 1 tablet daily"
                        },
                        new HealthProduct
                        {
                            Name = "Protein Powder",
                            Description = "High-quality whey protein for muscle recovery",
                            CategoryId = category.ID,
                            Price = 49.99M,
                            VendorId = "vendor14",
                            Sku = "HEALTH-PROTEIN-002",
                            Tags = ["health", "protein", "supplement"],
                            Ingredients = "Whey Protein Concentrate",
                            IsOrganic = false,
                            UsageInstructions = "Mix 1 scoop with water or milk"
                        }
                    });
                    break;

                case "Beauty":
                    products.AddRange(new[]
                    {
                        new BeautyProduct
                        {
                            Name = "Moisturizing Cream",
                            Description = "Hydrating cream for dry skin",
                            CategoryId = category.ID,
                            Price = 24.99M,
                            VendorId = "vendor15",
                            Sku = "BEAUTY-CREAM-001",
                            Tags = ["beauty", "moisturizer"],
                            Ingredients = "Shea Butter, Vitamin E",
                            SkinType = "Dry",
                            Volume = 100
                        },
                        new BeautyProduct
                        {
                            Name = "Sunscreen SPF 50",
                            Description = "Broad-spectrum sunscreen for UV protection",
                            CategoryId = category.ID,
                            Price = 19.99M,
                            VendorId = "vendor16",
                            Sku = "BEAUTY-SUNSCREEN-002",
                            Tags = ["beauty", "sunscreen"],
                            Ingredients = "Zinc Oxide, Aloe Vera",
                            SkinType = "All",
                            Volume = 200
                        }
                    });
                    break;

                case "Automotive":
                    products.AddRange(new[]
                    {
                        new AutomotiveProduct
                        {
                            Name = "Car Tire",
                            Description = "Durable all-season tire",
                            CategoryId = category.ID,
                            Price = 89.99M,
                            VendorId = "vendor17",
                            Sku = "AUTO-TIRE-001",
                            Tags = ["automotive", "tire"],
                            Brand = "Michelin",
                            Model = "X-Ice",
                            Year = 2023,
                            Compatibility = "Sedans"
                        },
                        new AutomotiveProduct
                        {
                            Name = "Car Battery",
                            Description = "Long-lasting car battery",
                            CategoryId = category.ID,
                            Price = 129.99M,
                            VendorId = "vendor18",
                            Sku = "AUTO-BATTERY-002",
                            Tags = ["automotive", "battery"],
                            Brand = "Bosch",
                            Model = "S6",
                            Year = 2024,
                            Compatibility = "SUVs, Trucks"
                        }
                    });
                    break;

                case "FoodAndBeverages":
                    products.AddRange(new[]
                    {
                        new FoodAndBeverageProduct
                        {
                            Name = "Organic Green Tea",
                            Description = "Refreshing pack of organic green tea bags",
                            CategoryId = category.ID,
                            Price = 9.99M,
                            VendorId = "vendor19",
                            Sku = "FOOD-TEA-001",
                            Tags = ["food", "tea"],
                            Ingredients = "Green Tea Leaves",
                            ExpiryDate = DateTime.UtcNow.AddYears(1),
                            IsVegetarian = true,
                            VolumeOrWeight = 200
                        },
                        new FoodAndBeverageProduct
                        {
                            Name = "Dark Chocolate Bar",
                            Description = "Rich dark chocolate with 70% cocoa",
                            CategoryId = category.ID,
                            Price = 4.99M,
                            VendorId = "vendor20",
                            Sku = "FOOD-CHOCOLATE-002",
                            Tags = ["food", "chocolate"],
                            Ingredients = "Cocoa, Sugar, Vanilla",
                            ExpiryDate = DateTime.UtcNow.AddMonths(6),
                            IsVegetarian = true,
                            VolumeOrWeight = 100
                        }
                    });
                    break;
                }
            }
            await DB.SaveAsync(products);
            Console.WriteLine("Products seeded successfully.");
        }
        else
        {
            Console.WriteLine("Products already exist. No seeding required.");
        }
    }
}
