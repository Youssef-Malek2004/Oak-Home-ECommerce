using Abstractions.ResultsPattern;
using MediatR;
using Payments.Domain.Entities;

namespace Payments.Application.CQRS.Queries;

public record GetUserPaymentsQuery(Guid UserId) : IRequest<Result<IEnumerable<Payment>>>;