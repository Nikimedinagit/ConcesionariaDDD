using Concesionaria.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Concesionaria.Infrastructure.Persistence;

internal sealed class ApplicationTransaction : IApplicationTransaction
{
    private readonly IDbContextTransaction _transaction;

    public ApplicationTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        _transaction.CommitAsync(cancellationToken);

    public Task RollbackAsync(CancellationToken cancellationToken = default) =>
        _transaction.RollbackAsync(cancellationToken);

    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
}
