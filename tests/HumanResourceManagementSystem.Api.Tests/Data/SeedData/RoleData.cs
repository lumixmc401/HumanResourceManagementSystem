using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlock.Core.Share.Helpers;
using HumanResourceManagementSystem.Api.Tests.Constants.Data;
using HumanResourceManagementSystem.Data.Models.HumanResource;

namespace HumanResourceManagementSystem.Api.Tests.Data.SeedData
{
    public static class RoleData
    {
        public static IReadOnlyList<Role> Roles =>
        [
            new()
            {
                Id = RoleConstants.Ids.RegularUserRoleId,
                Name = RoleConstants.Names.RegularUserRoleName,
            }
        ];
    }
}
