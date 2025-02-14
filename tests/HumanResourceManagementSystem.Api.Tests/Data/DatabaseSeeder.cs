using HumanResourceManagementSystem.Api.Tests.Data.SeedData;
using HumanResourceManagementSystem.Data.DbContexts;

namespace HumanResourceManagementSystem.Api.Tests.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedTestData(HumanResourceDbContext context)
        {
            await context.Roles.AddRangeAsync(RoleData.Roles);
            await context.Users.AddRangeAsync(UserData.Users);

            await context.SaveChangesAsync();
        }
    }
}
