using MongoDB.Entities;
using Products.Domain.Entities;

namespace Products.Infrastructure.Persistence;

public class Seedings
{
    public static async Task SeedCategoriesAsync()
    {
        // Check if the Categories collection is empty
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
}