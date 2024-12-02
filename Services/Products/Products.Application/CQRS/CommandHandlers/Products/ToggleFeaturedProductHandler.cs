using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class ToggleFeaturedProductCommandHandler(IProductsRepository productRepository)
    : IRequestHandler<ToggleFeaturedProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(ToggleFeaturedProductCommand request, CancellationToken cancellationToken)
    {
        return await productRepository.ToggleFeaturedStatus(request.ProductId);
    }
}