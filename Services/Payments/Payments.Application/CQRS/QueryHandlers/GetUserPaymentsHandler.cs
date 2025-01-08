using Abstractions.ResultsPattern;
using MediatR;
using Payments.Application.CQRS.Queries;
using Payments.Domain.Entities;
using Payments.Domain.Repositories;

namespace Payments.Application.CQRS.QueryHandlers;

public class GetUserPaymentsHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetUserPaymentsQuery, Result<IEnumerable<Payment>>>
{
    public async Task<Result<IEnumerable<Payment>>> Handle(GetUserPaymentsQuery request, CancellationToken cancellationToken)
    {
        return await paymentRepository.GetPaymentsByUserIdAsync(request.UserId, cancellationToken);
    }
}