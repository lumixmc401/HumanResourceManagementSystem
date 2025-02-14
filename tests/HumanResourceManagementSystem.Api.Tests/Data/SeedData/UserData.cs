using BuildingBlock.Core.Share.Helpers;
using HumanResourceManagementSystem.Api.Tests.Constants.Data;
using HumanResourceManagementSystem.Data.Models.HumanResource;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Text;

namespace HumanResourceManagementSystem.Api.Tests.Data.SeedData
{
    public static class UserData
    {
        private readonly static string adminSalt = "test_salt_1";
        private readonly static byte[] adminSaltbytes = Encoding.UTF8.GetBytes(adminSalt);
        private readonly static string regularUserSalt = "test_salt_2";
        private readonly static byte[] regularUserSaltbytes = Encoding.UTF8.GetBytes(regularUserSalt);
        public static IReadOnlyList<User> Users =>
        [
            new()
            {
                Id = UserConstants.Ids.AdminId,
                Email = UserConstants.Credentials.AdminEmail,
                PasswordHash = PasswordHelper.HashPassword(UserConstants.Credentials.AdminPassword, adminSaltbytes),
                Salt = Convert.ToBase64String(adminSaltbytes),
                UserRoles =
                [
                    new()
                    {
                        UserId = UserConstants.Ids.AdminId,
                        RoleId = RoleConstants.Ids.AdminRoleId
                    }
                ],
                UserClaims =
                [
                    new()
                    {
                        UserId = UserConstants.Ids.AdminId,
                        ClaimType = UserClaimConstants.Types.Name,
                        ClaimValue = UserClaimConstants.Values.AdminName,
                    }
                ]
            },
            new()
            {
                Id = UserConstants.Ids.RegularUserId,
                Email = UserConstants.Credentials.RegularUserEmail,
                PasswordHash = PasswordHelper.HashPassword(UserConstants.Credentials.RegularUserPassword, regularUserSaltbytes),
                Salt = Convert.ToBase64String(regularUserSaltbytes),
                UserRoles =
                [
                    new()
                    {
                        UserId = UserConstants.Ids.RegularUserId,
                        RoleId = RoleConstants.Ids.RegularUserRoleId
                    }
                ],
                UserClaims =
                [
                    new()
                    {
                        UserId = UserConstants.Ids.RegularUserId,
                        ClaimType = UserClaimConstants.Types.Name,
                        ClaimValue = UserClaimConstants.Values.RegularUserName,
                    }
                ]
            },
        ];
    }
}
