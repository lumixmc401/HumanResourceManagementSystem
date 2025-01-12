using HumanResourceManagementSystem.Data.Repositories.Interfaces;

namespace HumanResourceManagementSystem.Data.UnitOfWorks
{
    public interface IHumanResourceUnitOfWork : IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IClaimRepository Claims { get; }
        IUserRoleRepository UserRoles { get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserClaimRepository UserClaims { get; }
    }
}
