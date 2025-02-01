using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;

namespace HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource
{
    public interface IHumanResourceUnitOfWork : IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IUserRoleRepository UserRoles { get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserClaimRepository UserClaims { get; }
    }
}
