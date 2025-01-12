using System;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
    }
}
