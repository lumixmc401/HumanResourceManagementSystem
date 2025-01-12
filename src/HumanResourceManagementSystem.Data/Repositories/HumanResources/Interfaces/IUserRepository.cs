using HumanResourceManagementSystem.Data.Models.HumanResource;

namespace HumanResourceManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // 可以在這裡添加特定於 User 的方法
    }
}
