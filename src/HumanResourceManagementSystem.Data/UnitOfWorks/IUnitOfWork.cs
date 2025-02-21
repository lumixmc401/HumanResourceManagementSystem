using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace HumanResourceManagementSystem.Data.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
        Task<IDbContextTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
