using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.DTOs.ProductDtos;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class GetProductsByVendorIdHandler(IProductsRepository productsRepository) : IRequestHandler<GetProductsByVendorIdCommand, Result<IEnumerable<VendorGetProductsDto>>>
{
    public async Task<Result<IEnumerable<VendorGetProductsDto>>> Handle(GetProductsByVendorIdCommand request, CancellationToken cancellationToken)
    {
        return await productsRepository.GetProductsByVendorId(request.VendorId);
    }
}