using Shared.Contracts.Events.ProductEvents;
using Shared.Contracts.RequestDtosAndMappers.ProductDtos;

namespace Shared.Contracts.RequestDtosAndMappers.SharedMappers.ProductMappers;

public static class ProductMappersShared
{
    public static ProductCreated MapToProductCreated(string productId, CreateProductDto productDto, AddProductInventoryFields inventoryFields)
    {
        return new ProductCreated
        {
            ProductId = productId,
            VendorId = productDto.VendorId,
            WarehouseId = inventoryFields.WarehouseId,
            CategoryId = productDto.CategoryId,
            Price = productDto.Price
        };
    }
}