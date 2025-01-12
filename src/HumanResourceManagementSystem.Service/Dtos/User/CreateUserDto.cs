using HumanResourceManagementSystem.Service.Dtos.Claim;
using HumanResourceManagementSystem.Service.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Dtos.User
{
    public class CreateUserDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public IEnumerable<RoleDto> Roles { get; set; } = [];
        public IEnumerable<ClaimDto> Claims { get; set; } = [];
    }
}
