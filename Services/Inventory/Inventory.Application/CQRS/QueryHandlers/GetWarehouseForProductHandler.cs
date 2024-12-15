using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Queries;
using Inventory.Domain;
using Inventory.Domain.DTOs.WarehouseDtos;
using MediatR;

namespace Inventory.Application.CQRS.QueryHandlers;

public class GetWarehouseForProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetWarehouseForProductQuery, Result<WarehouseForProductDto>>
{
    public async Task<Result<WarehouseForProductDto>> Handle(GetWarehouseForProductQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.WarehouseRepository.GetWarehouseSellingProductDtoAsync(request.ProductId, cancellationToken);
    }
}