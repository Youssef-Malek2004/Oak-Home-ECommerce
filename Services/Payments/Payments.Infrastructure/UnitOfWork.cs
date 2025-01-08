using Abstractions.ResultsPattern;
using Payments.Domain;
using Payments.Infrastructure.Persistence;

namespace Payments.Infrastructure;

public class UnitOfWork(PaymentDbContext context) : IUnitOfWork
{
    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}