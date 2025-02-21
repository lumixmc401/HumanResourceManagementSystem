using HumanResourceManagementSystem.Data.DbContexts;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Implementations;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using HumanResourceManagementSystem.Data.Repositories.Implementations;

namespace HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource
{
    public class HumanResourceUnitOfWork(HumanResourceDbContext context) : UnitOfWork(context), IHumanResourceUnitOfWork
    {
        private readonly Lazy<IUserRepository> _users = new(() => new UserRepository(context));
        private readonly Lazy<IRoleRepository> _roles = new(() => new RoleRepository(context));
        private readonly Lazy<IPermissionRepository> _permissions = new(() => new PermissionRepository(context));
        private readonly Lazy<IUserRoleRepository> _userRoles = new(() => new UserRoleRepository(context));
        private readonly Lazy<IRolePermissionRepository> _rolePermissions = new(() => new RolePermissionRepository(context));
        private readonly Lazy<IUserClaimRepository> _userClaims = new(() => new UserClaimRepository(context));
        private readonly Lazy<IBillingRequestRepository> _billingRequests = new(() => new BillingRequestRepository(context));
        private readonly Lazy<IVoucherNumberRepository> _voucherNumbers = new(() => new VoucherNumberRepository(context));
        private readonly Lazy<IVoucherNumberSequenceRepository> _voucherNumberSequences = new(() => new VoucherNumberSequenceRepository(context));

        public IUserRepository Users => _users.Value;
        public IRoleRepository Roles => _roles.Value;
        public IPermissionRepository Permissions => _permissions.Value;
        public IUserRoleRepository UserRoles => _userRoles.Value;
        public IRolePermissionRepository RolePermissions => _rolePermissions.Value;
        public IUserClaimRepository UserClaims => _userClaims.Value;
        public IBillingRequestRepository BillingRequests => _billingRequests.Value;
        public IVoucherNumberRepository VoucherNumbers => _voucherNumbers.Value;
        public IVoucherNumberSequenceRepository VoucherNumberSequences => _voucherNumberSequences.Value;
    }
}
