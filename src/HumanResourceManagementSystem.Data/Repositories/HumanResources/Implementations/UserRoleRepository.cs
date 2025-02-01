using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.Implementations
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }
    }
}
