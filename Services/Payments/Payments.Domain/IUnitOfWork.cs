using Abstractions.ResultsPattern;

namespace Payments.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}