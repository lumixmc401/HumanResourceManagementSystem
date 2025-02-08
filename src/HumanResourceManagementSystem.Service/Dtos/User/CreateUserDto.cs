using HumanResourceManagementSystem.Service.DTOs.Role;
using HumanResourceManagementSystem.Service.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.User
{
    public class CreateUserDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public IEnumerable<RoleDto> Roles { get; set; } = [];
        public IEnumerable<UserClaimDto> Claims { get; set; } = [];
    }
}
