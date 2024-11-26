using Abstractions.ResultsPattern;
using Products.Domain.DTOs;
using Products.Domain.Entites;

namespace Products.Application.Services;

public interface IProductsRepository
{
    public Task<IEnumerable<Product>> GetProducts();
    public Task<Result<Product>> CreateProduct(CreateProductDto product);
}