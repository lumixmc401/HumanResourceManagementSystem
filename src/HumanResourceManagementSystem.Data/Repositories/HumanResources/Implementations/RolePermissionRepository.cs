using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.Implementations
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(DbContext context) : base(context)
        {
        }
    }
}
