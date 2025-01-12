using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.Implementations;
using HumanResourceManagementSystem.Data.Repositories.Interfaces;

namespace HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource
{
    public class HumanResourceUnitOfWork : UnitOfWork, IHumanResourceUnitOfWork
    {
        public HumanResourceUnitOfWork(HumanResourceDbContext context) : base(context)
        {
            _users = new Lazy<IUserRepository>(() => new UserRepository(context));
            _roles = new Lazy<IRoleRepository>(() => new RoleRepository(context));
            _permissions = new Lazy<IPermissionRepository>(() => new PermissionRepository(context));
            _claims = new Lazy<IClaimRepository>(() => new ClaimRepository(context));
            _userRoles = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(context));
            _rolePermissions = new Lazy<IRolePermissionRepository>(() => new RolePermissionRepository(context));
            _userClaims = new Lazy<IUserClaimRepository>(() => new UserClaimRepository(context));
        }

        private readonly Lazy<IUserRepository> _users;
        private readonly Lazy<IRoleRepository> _roles;
        private readonly Lazy<IPermissionRepository> _permissions;
        private readonly Lazy<IClaimRepository> _claims;
        private readonly Lazy<IUserRoleRepository> _userRoles;
        private readonly Lazy<IRolePermissionRepository> _rolePermissions;
        private readonly Lazy<IUserClaimRepository> _userClaims;

        public IUserRepository Users => _users.Value;
        public IRoleRepository Roles => _roles.Value;
        public IPermissionRepository Permissions => _permissions.Value;
        public IClaimRepository Claims => _claims.Value;
        public IUserRoleRepository UserRoles => _userRoles.Value;
        public IRolePermissionRepository RolePermissions => _rolePermissions.Value;
        public IUserClaimRepository UserClaims => _userClaims.Value;
    }
}
