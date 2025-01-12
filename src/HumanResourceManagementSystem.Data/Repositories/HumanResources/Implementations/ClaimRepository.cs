using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.Implementations
{
    public class ClaimRepository : Repository<Claim>, IClaimRepository
    {
        public ClaimRepository(DbContext context) : base(context)
        {
        }
    }
}
