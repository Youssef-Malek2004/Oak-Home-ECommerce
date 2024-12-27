using System.Linq.Expressions;
using Abstractions.ResultsPattern;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Services.Data;
using Payments.Domain.Entities;
using Payments.Domain.Errors;
using Payments.Domain.Repositories;

namespace Payments.Infrastructure.Persistence.Repositories;

public class PaymentRepository(IPaymentDbContext dbContext) : IPaymentRepository
{
    public async Task<Result<IEnumerable<Payment>>> GetPaymentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await dbContext.Payments.ToListAsync(cancellationToken);
            return Result<IEnumerable<Payment>>.Success(payments);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Payment>>.Failure(PaymentErrors.PaymentQueryFailed);
        }
    }

    public async Task<Result<Payment?>> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (payment == null)
        {
            return Result<Payment?>.Failure(PaymentErrors.PaymentNotFoundId(id));
        }

        return Result<Payment?>.Success(payment);
    }

    public async Task<Result<IEnumerable<Payment>>> GetPaymentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await dbContext.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Payment>>.Success(payments);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Payment>>.Failure(PaymentErrors.PaymentQueryFailed);
        }
    }

    public async Task<Result> AddPaymentAsync(Payment payment)
    {
        try
        {
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(PaymentErrors.PaymentAddFailed);
        }
    }

    public async Task<Result> RemovePaymentAsync(Payment payment)
    {
        try
        {
            dbContext.Payments.Remove(payment);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(PaymentErrors.PaymentRemoveFailed);
        }
    }

    public async Task<Result> UpdatePaymentAsync(Payment payment)
    {
        try
        {
            dbContext.Payments.Update(payment);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(PaymentErrors.PaymentUpdateFailed);
        }
    }

    public async Task<Result<IEnumerable<Payment>>> GetPaymentsByConditionAsync(
        Expression<Func<Payment, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var payments = await dbContext.Payments
                .Where(predicate)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<Payment>>.Success(payments);
        }
        catch (Exception)
        {
            return Result<IEnumerable<Payment>>.Failure(PaymentErrors.PaymentQueryFailed);
        }
    }
}
