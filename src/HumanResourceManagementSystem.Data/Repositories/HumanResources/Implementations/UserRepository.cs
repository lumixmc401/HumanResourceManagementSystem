using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.HumanResources.Implementations
{
    public class UserRepository(DbContext context) : Repository<User>(context), IUserRepository
    {
        public Task<User?> GetUserByEmailAsync(string email)
        {
            return Query().Where(w => w.Email == email)
                          .Include(i => i.UserRoles)
                          .Include(i => i.UserClaims)
                          .FirstOrDefaultAsync();
        }
    }
}
