using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Api.Tests.Constants.Data
{
    public static class UserConstants
    {
        public static class Ids
        {
            public static readonly Guid AdminId = new("11111111-1111-1111-1111-111111111111");
            public static readonly Guid RegularUserId = new("22222222-2222-2222-2222-222222222222");
        }
        public static class Credentials
        {
            public const string AdminEmail = "admin111@example.com";
            public const string AdminPassword = "Admin123!";
            public const string RegularUserEmail = "user111@example.com";
            public const string RegularUserPassword = "User123!";
        }
    }
}
