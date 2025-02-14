using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Api.Tests.Constants.Data
{
    public static class RoleConstants
    {
        public static class Ids
        {
            public static readonly Guid AdminRoleId = new("d2b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d");
            public static readonly Guid RegularUserRoleId = new("d3b7f5e1-5c3b-4d3a-8b1e-2f3b5e1c3b4d");
        }
        public static class Names
        { 
            public const string AdminRoleName = "Admin";
            public const string RegularUserRoleName = "RegularUser";
        }
    }
}
