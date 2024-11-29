using Abstractions.ResultsPattern;
using MediatR;
using Products.Application.CQRS.CommandsAndQueries.Products;
using Products.Application.Services;
using Products.Domain.Entities;

namespace Products.Application.CQRS.CommandHandlers.Products;

public class UpdateProductHandler(IProductsRepository repository)
    : IRequestHandler<UpdateProductCommand, Result<Product>>
{
    public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return await repository.UpdateProduct(request.Id, request.Product);
    }
}
