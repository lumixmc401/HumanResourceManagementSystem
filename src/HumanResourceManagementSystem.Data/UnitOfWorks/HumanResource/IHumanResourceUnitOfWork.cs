using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;

namespace HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource
{
    public interface IHumanResourceUnitOfWork : IUnitOfWork
    {
        IBillingRequestRepository BillingRequests { get;}
        IVoucherNumberRepository VoucherNumbers { get; }
        IVoucherNumberSequenceRepository VoucherNumberSequences { get; }
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IUserRoleRepository UserRoles { get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserClaimRepository UserClaims { get; }
    }
}
