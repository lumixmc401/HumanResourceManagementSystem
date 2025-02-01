using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.HumanResources.Implementations
{
    public class UserClaimRepository(DbContext context) : Repository<UserClaim>(context), IUserClaimRepository
    {
    }
}
