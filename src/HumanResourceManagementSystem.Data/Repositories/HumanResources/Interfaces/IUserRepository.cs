using HumanResourceManagementSystem.Data.Models.HumanResource;

namespace HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
