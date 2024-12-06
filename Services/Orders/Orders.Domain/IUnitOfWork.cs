using Abstractions.ResultsPattern;
using Orders.Domain.Repositories;

namespace Orders.Domain;

public interface IUnitOfWork : IDisposable
{
    IOrdersRepository OrdersRepository { get; }
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken = default);
}