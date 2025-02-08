using HumanResourceManagementSystem.Service.DTOs.Role;
using HumanResourceManagementSystem.Service.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.User
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public IEnumerable<RoleDto> Roles { get; set; } = [];
        public IEnumerable<UserClaimDto> Claims { get; set; } = [];
    }
}
