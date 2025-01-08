using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Payments.Domain.Entities;

namespace Payments.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Result<IEnumerable<Payment>>> GetPaymentsAsync(CancellationToken cancellationToken = default);
    Task<Result<Payment?>> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> AddPaymentAsync(Payment payment);
    Task<Result> RemovePaymentAsync(Payment payment);
    Task<Result> UpdatePaymentAsync(Payment payment);
    Task<Result<IEnumerable<Payment>>> GetPaymentsByConditionAsync(
        Expression<Func<Payment, bool>> predicate,
        CancellationToken cancellationToken = default);
}