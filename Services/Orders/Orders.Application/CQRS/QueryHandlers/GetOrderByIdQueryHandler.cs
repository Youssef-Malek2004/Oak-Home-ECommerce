using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Queries;
using Orders.Domain.DTOs.Orders;
using Orders.Domain.Mappers;
using Orders.Domain.Repositories;

namespace Orders.Application.CQRS.QueryHandlers;

public class GetOrderByIdQueryHandler(IOrdersRepository repository)
    : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderResult = await repository.GetOrderByIdAsync(request.OrderId, cancellationToken);
        if (!orderResult.IsSuccess || orderResult.Value == null)
        {
            return Result<OrderResponse>.Failure(orderResult.Error);
        }

        return Result<OrderResponse>.Success(orderResult.Value!.ToOrderResponse());
    }
}
