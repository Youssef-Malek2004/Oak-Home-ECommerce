using Abstractions.ResultsPattern;
using MediatR;
using Orders.Application.CQRS.Commands;
using Orders.Domain;
using Orders.Domain.Repositories;

namespace Orders.Application.CQRS.CommandHandlers;

public class DeleteOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand, Result>
{
    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderResult = await unitOfWork.OrdersRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
        if (!orderResult.IsSuccess || orderResult.Value == null)
        {
            return Result.Failure(orderResult.Error);
        }

        var deleteResult = await unitOfWork.OrdersRepository.RemoveOrderAsync(orderResult.Value!);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return deleteResult;
    }
}
