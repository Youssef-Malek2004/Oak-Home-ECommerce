using System.Text.Json;
using Abstractions.ResultsPattern;
using MongoDB.Entities;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;
using Products.Domain.Entities.Products;
using Products.Domain.Errors;
using Shared.Contracts.RequestDtosAndMappers.ProductDtos;

namespace Products.Domain.Mappers;

public static class ProductMappers
{
    public static async Task<Result<Product>> MapCreateProductDtoToProductAsync(CreateProductDto createProductDto, IDictionary<string, object>? dynamicFields = null)
{
    var category = await DB.Find<Category>().Match(c => c.ID == createProductDto.CategoryId).ExecuteFirstAsync();
    if (category == null)
    {
        return Result<Product>.Failure(CategoryErrors.CategoryNotFoundId(createProductDto.CategoryId));
    }

    var result = MapDynamicFieldsToProduct(category, dynamicFields);

    if (result.IsFailure) return result;

    var product = result.Value;

    if (product is null) return Result<Product>.Failure(ProductErrors.ProductAddFailed("Product is Null"));

    product.Name = createProductDto.Name;
    product.Description = createProductDto.Description;
    product.CreatedAt = DateTime.UtcNow;
    product.UpdatedAt = DateTime.UtcNow;
    product.CategoryId = createProductDto.CategoryId;
    product.Price = createProductDto.Price;
    product.VendorId = createProductDto.VendorId;
    product.ImageUrls = createProductDto.ImageUrls;
    product.Sku = createProductDto.Sku;
    product.Tags = createProductDto.Tags;

    return Result<Product>.Success(product);
}

    public static void MapUpdateProductDtoToProduct(UpdateProductDto productDto, Product product)
    {
        if (!string.IsNullOrEmpty(productDto.Name))
            product.Name = productDto.Name;

        if (!string.IsNullOrEmpty(productDto.Description))
            product.Description = productDto.Description;

        if (productDto.Price.HasValue)
            product.Price = productDto.Price.Value;

        if (!string.IsNullOrEmpty(productDto.Sku))
            product.Sku = productDto.Sku;

        if (!string.IsNullOrEmpty(productDto.VendorId))
            product.VendorId = productDto.VendorId;

        if (!string.IsNullOrEmpty(productDto.CategoryId))
            product.CategoryId = productDto.CategoryId;

        if (productDto.ImageUrls != null && productDto.ImageUrls.Any())
            product.ImageUrls = productDto.ImageUrls;
        
        product.Featured = productDto.Featured;
        product.UpdatedAt = DateTime.UtcNow;
    }
    public static Result<Product> MapDynamicFieldsToProduct(Category category, IDictionary<string, object>? dynamicFields)
    {
        if (dynamicFields == null)
        {
            return Result<Product>.Failure(ProductValidationErrors.NullDynamicFields);
        }

        return category.Name switch
        {
            "Electronics" => MapElectronicsProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapElectronicsProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapElectronicsProduct(dynamicFields).Error),
                
            "Clothing" => MapClothingProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapClothingProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapClothingProduct(dynamicFields).Error),
                
            "Books" => MapBookProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapBookProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapBookProduct(dynamicFields).Error),
                
            "HomeAppliances" => MapHomeAppliancesProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapHomeAppliancesProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapHomeAppliancesProduct(dynamicFields).Error),
                
            "Toys" => MapToyProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapToyProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapToyProduct(dynamicFields).Error),
                
            "Sports" => MapSportsProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapSportsProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapSportsProduct(dynamicFields).Error),
                
            "Health" => MapHealthProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapHealthProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapHealthProduct(dynamicFields).Error),
                
            "Beauty" => MapBeautyProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapBeautyProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapBeautyProduct(dynamicFields).Error),
                
            "Automotive" => MapAutomotiveProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapAutomotiveProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapAutomotiveProduct(dynamicFields).Error),
                
            "FoodAndBeverages" => MapFoodAndBeverageProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapFoodAndBeverageProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapFoodAndBeverageProduct(dynamicFields).Error),
                
            "Wood" => MapWoodProduct(dynamicFields).IsSuccess
                ? Result<Product>.Success(MapWoodProduct(dynamicFields).Value!)
                : Result<Product>.Failure(MapWoodProduct(dynamicFields).Error),
                
            _ => Result<Product>.Failure(ProductValidationErrors.UnsupportedCategory(category.Name))
        };
    }

    private static Result<ElectronicsProduct> MapElectronicsProduct(IDictionary<string, object> dynamicFields)
    {
        var brandResult = GetRequiredField(dynamicFields, "Brand");
        var modelResult = GetRequiredField(dynamicFields, "Model");
        var warrantyPeriodResult = GetOptionalField<int>(dynamicFields, "WarrantyPeriod");

        if (!brandResult.IsSuccess) return Result<ElectronicsProduct>.Failure(brandResult.Error);
        if (!modelResult.IsSuccess) return Result<ElectronicsProduct>.Failure(modelResult.Error);
        if (!warrantyPeriodResult.IsSuccess) return Result<ElectronicsProduct>.Failure(warrantyPeriodResult.Error);

        return Result<ElectronicsProduct>.Success(new ElectronicsProduct
        {
            Brand = brandResult.Value!,
            Model = modelResult.Value!,
            WarrantyPeriod = warrantyPeriodResult.Value
        });
    }

    private static Result<ClothingProduct> MapClothingProduct(IDictionary<string, object> dynamicFields)
    {
        var sizeResult = GetRequiredField(dynamicFields, "Size");
        var materialResult = GetRequiredField(dynamicFields, "Material");
        var genderResult = GetRequiredField(dynamicFields, "Gender");

        if (!sizeResult.IsSuccess) return Result<ClothingProduct>.Failure(sizeResult.Error);
        if (!materialResult.IsSuccess) return Result<ClothingProduct>.Failure(materialResult.Error);
        if (!genderResult.IsSuccess) return Result<ClothingProduct>.Failure(genderResult.Error);

        return Result<ClothingProduct>.Success(new ClothingProduct
        {
            Size = sizeResult.Value!,
            Material = materialResult.Value!,
            Gender = genderResult.Value!
        });
    }
    
    private static Result<BookProduct> MapBookProduct(IDictionary<string, object> dynamicFields)
    {
        var authorResult = GetRequiredField(dynamicFields, "Author");
        var publisherResult = GetRequiredField(dynamicFields, "Publisher");
        var pagesResult = GetOptionalField<int>(dynamicFields, "Pages");
        var isbnResult = GetRequiredField(dynamicFields, "Isbn");

        if (!authorResult.IsSuccess) return Result<BookProduct>.Failure(authorResult.Error);
        if (!publisherResult.IsSuccess) return Result<BookProduct>.Failure(publisherResult.Error);
        if (!pagesResult.IsSuccess) return Result<BookProduct>.Failure(pagesResult.Error);
        if (!isbnResult.IsSuccess) return Result<BookProduct>.Failure(isbnResult.Error);

        return Result<BookProduct>.Success(new BookProduct
        {
            Author = authorResult.Value!,
            Publisher = publisherResult.Value!,
            Pages = pagesResult.Value,
            Isbn = isbnResult.Value!
        });
    }
    
    private static Result<HomeAppliancesProduct> MapHomeAppliancesProduct(IDictionary<string, object> dynamicFields)
    {
        var brandResult = GetRequiredField(dynamicFields, "Brand");
        var powerConsumptionResult = GetOptionalField<int>(dynamicFields, "PowerConsumption");
        var isEnergyEfficientResult = GetOptionalField<bool>(dynamicFields, "IsEnergyEfficient");

        if (!brandResult.IsSuccess) return Result<HomeAppliancesProduct>.Failure(brandResult.Error);
        if (!powerConsumptionResult.IsSuccess) return Result<HomeAppliancesProduct>.Failure(powerConsumptionResult.Error);
        if (!isEnergyEfficientResult.IsSuccess) return Result<HomeAppliancesProduct>.Failure(isEnergyEfficientResult.Error);

        return Result<HomeAppliancesProduct>.Success(new HomeAppliancesProduct
        {
            Brand = brandResult.Value!,
            PowerConsumption = powerConsumptionResult.Value,
            IsEnergyEfficient = isEnergyEfficientResult.Value
        });
    }
    private static Result<ToyProduct> MapToyProduct(IDictionary<string, object> dynamicFields)
    {
        var minimumAgeResult = GetOptionalField<int>(dynamicFields, "MinimumAge");
        var materialResult = GetRequiredField(dynamicFields, "Material");
        var isEducationalResult = GetOptionalField<bool>(dynamicFields, "IsEducational");

        if (!minimumAgeResult.IsSuccess) return Result<ToyProduct>.Failure(minimumAgeResult.Error);
        if (!materialResult.IsSuccess) return Result<ToyProduct>.Failure(materialResult.Error);
        if (!isEducationalResult.IsSuccess) return Result<ToyProduct>.Failure(isEducationalResult.Error);

        return Result<ToyProduct>.Success(new ToyProduct
        {
            MinimumAge = minimumAgeResult.Value,
            Material = materialResult.Value!,
            IsEducational = isEducationalResult.Value
        });
    }

    private static Result<SportsProduct> MapSportsProduct(IDictionary<string, object> dynamicFields)
    {
        var sportTypeResult = GetRequiredField(dynamicFields, "SportType");
        var brandResult = GetRequiredField(dynamicFields, "Brand");
        var sizeResult = GetRequiredField(dynamicFields, "Size");

        if (!sportTypeResult.IsSuccess) return Result<SportsProduct>.Failure(sportTypeResult.Error);
        if (!brandResult.IsSuccess) return Result<SportsProduct>.Failure(brandResult.Error);
        if (!sizeResult.IsSuccess) return Result<SportsProduct>.Failure(sizeResult.Error);

        return Result<SportsProduct>.Success(new SportsProduct
        {
            SportType = sportTypeResult.Value!,
            Brand = brandResult.Value!,
            Size = sizeResult.Value!
        });
    }

    private static Result<HealthProduct> MapHealthProduct(IDictionary<string, object> dynamicFields)
    {
        var ingredientsResult = GetRequiredField(dynamicFields, "Ingredients");
        var isOrganicResult = GetOptionalField<bool>(dynamicFields, "IsOrganic");
        var usageInstructionsResult = GetRequiredField(dynamicFields, "UsageInstructions");

        if (!ingredientsResult.IsSuccess) return Result<HealthProduct>.Failure(ingredientsResult.Error);
        if (!isOrganicResult.IsSuccess) return Result<HealthProduct>.Failure(isOrganicResult.Error);
        if (!usageInstructionsResult.IsSuccess) return Result<HealthProduct>.Failure(usageInstructionsResult.Error);

        return Result<HealthProduct>.Success(new HealthProduct
        {
            Ingredients = ingredientsResult.Value!,
            IsOrganic = isOrganicResult.Value,
            UsageInstructions = usageInstructionsResult.Value!
        });
    }

    private static Result<BeautyProduct> MapBeautyProduct(IDictionary<string, object> dynamicFields)
    {
        var ingredientsResult = GetRequiredField(dynamicFields, "Ingredients");
        var skinTypeResult = GetRequiredField(dynamicFields, "SkinType");
        var volumeResult = GetOptionalField<int>(dynamicFields, "Volume");

        if (!ingredientsResult.IsSuccess) return Result<BeautyProduct>.Failure(ingredientsResult.Error);
        if (!skinTypeResult.IsSuccess) return Result<BeautyProduct>.Failure(skinTypeResult.Error);
        if (!volumeResult.IsSuccess) return Result<BeautyProduct>.Failure(volumeResult.Error);

        return Result<BeautyProduct>.Success(new BeautyProduct
        {
            Ingredients = ingredientsResult.Value!,
            SkinType = skinTypeResult.Value!,
            Volume = volumeResult.Value
        });
    }

    private static Result<AutomotiveProduct> MapAutomotiveProduct(IDictionary<string, object> dynamicFields)
    {
        var brandResult = GetRequiredField(dynamicFields, "Brand");
        var modelResult = GetRequiredField(dynamicFields, "Model");
        var yearResult = GetOptionalField<int>(dynamicFields, "Year");
        var compatibilityResult = GetRequiredField(dynamicFields, "Compatibility");

        if (!brandResult.IsSuccess) return Result<AutomotiveProduct>.Failure(brandResult.Error);
        if (!modelResult.IsSuccess) return Result<AutomotiveProduct>.Failure(modelResult.Error);
        if (!yearResult.IsSuccess) return Result<AutomotiveProduct>.Failure(yearResult.Error);
        if (!compatibilityResult.IsSuccess) return Result<AutomotiveProduct>.Failure(compatibilityResult.Error);

        return Result<AutomotiveProduct>.Success(new AutomotiveProduct
        {
            Brand = brandResult.Value!,
            Model = modelResult.Value!,
            Year = yearResult.Value,
            Compatibility = compatibilityResult.Value!
        });
    }

    private static Result<FoodAndBeverageProduct> MapFoodAndBeverageProduct(IDictionary<string, object> dynamicFields)
    {
        var ingredientsResult = GetRequiredField(dynamicFields, "Ingredients");
        var expiryDateResult = GetOptionalField<DateTime>(dynamicFields, "ExpiryDate");
        var isVegetarianResult = GetOptionalField<bool>(dynamicFields, "IsVegetarian");
        var volumeOrWeightResult = GetOptionalField<int>(dynamicFields, "VolumeOrWeight");

        if (!ingredientsResult.IsSuccess) return Result<FoodAndBeverageProduct>.Failure(ingredientsResult.Error);
        if (!expiryDateResult.IsSuccess) return Result<FoodAndBeverageProduct>.Failure(expiryDateResult.Error);
        if (!isVegetarianResult.IsSuccess) return Result<FoodAndBeverageProduct>.Failure(isVegetarianResult.Error);
        if (!volumeOrWeightResult.IsSuccess) return Result<FoodAndBeverageProduct>.Failure(volumeOrWeightResult.Error);

        return Result<FoodAndBeverageProduct>.Success(new FoodAndBeverageProduct
        {
            Ingredients = ingredientsResult.Value!,
            ExpiryDate = expiryDateResult.Value,
            IsVegetarian = isVegetarianResult.Value,
            VolumeOrWeight = volumeOrWeightResult.Value
        });
    }
    
private static Result<WoodProduct> MapWoodProduct(IDictionary<string, object> dynamicFields)
{
    var materialResult = GetRequiredField(dynamicFields, "Material");
    var finishResult = GetRequiredField(dynamicFields, "Finish");
    var lengthResult = GetOptionalField<decimal>(dynamicFields, "Length");
    var widthResult = GetOptionalField<decimal>(dynamicFields, "Width");
    var heightResult = GetOptionalField<decimal>(dynamicFields, "Height");
    var weightResult = GetOptionalField<decimal>(dynamicFields, "Weight");
    var colorResult = GetRequiredField(dynamicFields, "Color");
    var subCategoryResult = GetRequiredField(dynamicFields, "SubCategory");
    var usageResult = GetRequiredField(dynamicFields, "Usage");
    var brandResult = GetRequiredField(dynamicFields, "Brand");
    var manufacturerResult = GetRequiredField(dynamicFields, "Manufacturer");
    var countryOfOriginResult = GetRequiredField(dynamicFields, "CountryOfOrigin");
    
    var isCustomizableResult = GetOptionalField<bool>(dynamicFields, "IsCustomizable");
    var featuresResult = GetOptionalField<string>(dynamicFields, "Features");
    var warrantyInYearsResult = GetOptionalField<int>(dynamicFields, "WarrantyInYears");
    var maintenanceInstructionsResult = GetOptionalField<string>(dynamicFields, "MaintenanceInstructions");
    var manufactureDateResult = GetOptionalField<DateTime>(dynamicFields, "ManufactureDate");
    var isEcoFriendlyResult = GetOptionalField<bool>(dynamicFields, "IsEcoFriendly");
    
    if (!materialResult.IsSuccess) return Result<WoodProduct>.Failure(materialResult.Error);
    if (!finishResult.IsSuccess) return Result<WoodProduct>.Failure(finishResult.Error);
    if (!lengthResult.IsSuccess) return Result<WoodProduct>.Failure(lengthResult.Error);
    if (!widthResult.IsSuccess) return Result<WoodProduct>.Failure(widthResult.Error);
    if (!heightResult.IsSuccess) return Result<WoodProduct>.Failure(heightResult.Error);
    if (!weightResult.IsSuccess) return Result<WoodProduct>.Failure(weightResult.Error);
    if (!colorResult.IsSuccess) return Result<WoodProduct>.Failure(colorResult.Error);
    if (!subCategoryResult.IsSuccess) return Result<WoodProduct>.Failure(subCategoryResult.Error);
    if (!usageResult.IsSuccess) return Result<WoodProduct>.Failure(usageResult.Error);
    if (!brandResult.IsSuccess) return Result<WoodProduct>.Failure(brandResult.Error);
    if (!manufacturerResult.IsSuccess) return Result<WoodProduct>.Failure(manufacturerResult.Error);
    if (!countryOfOriginResult.IsSuccess) return Result<WoodProduct>.Failure(countryOfOriginResult.Error);
    
    return Result<WoodProduct>.Success(new WoodProduct
    {
        Material = materialResult.Value!,
        Finish = finishResult.Value!,
        Length = lengthResult.Value,
        Width = widthResult.Value,
        Height = heightResult.Value,
        Weight = weightResult.Value,
        Color = colorResult.Value!,
        SubCategory = subCategoryResult.Value!,
        Usage = usageResult.Value!,
        Brand = brandResult.Value!,
        Manufacturer = manufacturerResult.Value!,
        CountryOfOrigin = countryOfOriginResult.Value!,
        IsCustomizable = isCustomizableResult.IsSuccess ? isCustomizableResult.Value : false,
        Features = featuresResult.IsSuccess ? featuresResult.Value! : string.Empty,
        WarrantyInYears = warrantyInYearsResult.IsSuccess ? warrantyInYearsResult.Value : 0,
        MaintenanceInstructions = maintenanceInstructionsResult.IsSuccess ? maintenanceInstructionsResult.Value! : string.Empty,
        ManufactureDate = manufactureDateResult.IsSuccess ? manufactureDateResult.Value : DateTime.MinValue,
        IsEcoFriendly = isEcoFriendlyResult.IsSuccess && isEcoFriendlyResult.Value
    });
}


    private static Result<string> GetRequiredField(IDictionary<string, object> dynamicFields, string key)
    {
        if (!dynamicFields.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return Result<string>.Failure(ProductValidationErrors.MissingField(key));
        }
        return Result<string>.Success(value.ToString()!);
    }

    private static Result<T> GetOptionalField<T>(IDictionary<string, object> dynamicFields, string key)
    {
        if (!dynamicFields.TryGetValue(key, out var value))
            return Result<T>.Failure(ProductValidationErrors.MissingField(key));
        try
        {
            if (typeof(T) == typeof(int))
            {
                return Result<T>.Success((T)(object)GetInt(value));
            }
            if (typeof(T) == typeof(decimal))
            {
                return Result<T>.Success((T)(object)GetDecimal(value));
            }
            if (typeof(T) == typeof(bool))
            {
                return Result<T>.Success((T)(object)GetBool(value));
            }
            if (typeof(T) == typeof(DateTime))
            {
                return Result<T>.Success((T)(object)GetDateTime(value));
            }
            return Result<T>.Success((T)Convert.ChangeType(value, typeof(T)));
        }
        catch (InvalidCastException)
        {
            return Result<T>.Failure(ProductValidationErrors.InvalidField(key));
        }
        catch (FormatException)
        {
            return Result<T>.Failure(ProductValidationErrors.InvalidField(key));
        }
    }
    private static decimal GetDecimal(object value)
    {
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
        {
            return jsonElement.GetDecimal();
        }
        return Convert.ToDecimal(value);
    }
    private static int GetInt(object value)
    {
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
        {
            return jsonElement.GetInt32();
        }
        return Convert.ToInt32(value);
    }
    private static bool GetBool(object value)
    {
        if (value is JsonElement jsonElement && (jsonElement.ValueKind == JsonValueKind.True || jsonElement.ValueKind == JsonValueKind.False))
        {
            return jsonElement.GetBoolean();
        }
        return Convert.ToBoolean(value);
    }
    
    private static DateTime GetDateTime(object value)
    {
        if (value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.String)
        {
            if (DateTime.TryParse(jsonElement.GetString(), out var dateTime))
            {
                return dateTime;
            }
            throw new FormatException($"Invalid date format: {jsonElement.GetString()}");
        }

        if (value is string stringValue)
        {
            if (DateTime.TryParse(stringValue, out var dateTime))
            {
                return dateTime;
            }
            throw new FormatException($"Invalid date format: {stringValue}");
        }

        return Convert.ToDateTime(value);
    }

    public static Product CopyDynamicFieldsToExistingProduct(Product existingProduct, Product updatedProduct)
    {
        switch (existingProduct)
        {
            case ElectronicsProduct electronics when updatedProduct is ElectronicsProduct updatedElectronics:
                electronics.Brand = updatedElectronics.Brand;
                electronics.Model = updatedElectronics.Model;
                electronics.WarrantyPeriod = updatedElectronics.WarrantyPeriod;
                break;

            case ClothingProduct clothing when updatedProduct is ClothingProduct updatedClothing:
                clothing.Size = updatedClothing.Size;
                clothing.Material = updatedClothing.Material;
                clothing.Gender = updatedClothing.Gender;
                break;

            case BookProduct book when updatedProduct is BookProduct updatedBook:
                book.Author = updatedBook.Author;
                book.Publisher = updatedBook.Publisher;
                book.Pages = updatedBook.Pages;
                book.Isbn = updatedBook.Isbn;
                break;

            case HomeAppliancesProduct homeAppliance when updatedProduct is HomeAppliancesProduct updatedHomeAppliance:
                homeAppliance.Brand = updatedHomeAppliance.Brand;
                homeAppliance.PowerConsumption = updatedHomeAppliance.PowerConsumption;
                homeAppliance.IsEnergyEfficient = updatedHomeAppliance.IsEnergyEfficient;
                break;

            case ToyProduct toy when updatedProduct is ToyProduct updatedToy:
                toy.MinimumAge = updatedToy.MinimumAge;
                toy.Material = updatedToy.Material;
                toy.IsEducational = updatedToy.IsEducational;
                break;

            case SportsProduct sports when updatedProduct is SportsProduct updatedSports:
                sports.SportType = updatedSports.SportType;
                sports.Brand = updatedSports.Brand;
                sports.Size = updatedSports.Size;
                break;

            case HealthProduct health when updatedProduct is HealthProduct updatedHealth:
                health.Ingredients = updatedHealth.Ingredients;
                health.IsOrganic = updatedHealth.IsOrganic;
                health.UsageInstructions = updatedHealth.UsageInstructions;
                break;

            case BeautyProduct beauty when updatedProduct is BeautyProduct updatedBeauty:
                beauty.Ingredients = updatedBeauty.Ingredients;
                beauty.SkinType = updatedBeauty.SkinType;
                beauty.Volume = updatedBeauty.Volume;
                break;

            case AutomotiveProduct automotive when updatedProduct is AutomotiveProduct updatedAutomotive:
                automotive.Brand = updatedAutomotive.Brand;
                automotive.Model = updatedAutomotive.Model;
                automotive.Year = updatedAutomotive.Year;
                automotive.Compatibility = updatedAutomotive.Compatibility;
                break;

            case FoodAndBeverageProduct food when updatedProduct is FoodAndBeverageProduct updatedFood:
                food.Ingredients = updatedFood.Ingredients;
                food.ExpiryDate = updatedFood.ExpiryDate;
                food.IsVegetarian = updatedFood.IsVegetarian;
                food.VolumeOrWeight = updatedFood.VolumeOrWeight;
                break;
            
            case WoodProduct wood when updatedProduct is WoodProduct updatedWood:
                wood.Material = updatedWood.Material;
                wood.Finish = updatedWood.Finish;
                wood.Length = updatedWood.Length;
                wood.Width = updatedWood.Width;
                wood.Height = updatedWood.Height;
                wood.Weight = updatedWood.Weight;
                wood.Color = updatedWood.Color;
                wood.SubCategory = updatedWood.SubCategory;
                wood.Usage = updatedWood.Usage;
                wood.IsCustomizable = updatedWood.IsCustomizable;
                wood.Features = updatedWood.Features;
                wood.WarrantyInYears = updatedWood.WarrantyInYears;
                wood.MaintenanceInstructions = updatedWood.MaintenanceInstructions;
                wood.Brand = updatedWood.Brand;
                wood.Manufacturer = updatedWood.Manufacturer;
                wood.ManufactureDate = updatedWood.ManufactureDate;
                wood.CountryOfOrigin = updatedWood.CountryOfOrigin;
                wood.IsEcoFriendly = updatedWood.IsEcoFriendly;
                break;

        }

        return existingProduct;
    }

}