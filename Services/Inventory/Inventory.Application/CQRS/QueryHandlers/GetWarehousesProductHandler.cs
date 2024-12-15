using Abstractions.ResultsPattern;
using Inventory.Application.CQRS.Queries;
using Inventory.Domain;
using Inventory.Domain.DTOs.WarehouseDtos;
using MediatR;

namespace Inventory.Application.CQRS.QueryHandlers;

public class GetWarehousesProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetWarehousesProductQuery, Result<IEnumerable<WarehouseNamesDto>>>
{
    public async Task<Result<IEnumerable<WarehouseNamesDto>>> Handle(GetWarehousesProductQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.WarehouseRepository.GetWarehouseIdsAndNamesAsync(cancellationToken);
    }
}