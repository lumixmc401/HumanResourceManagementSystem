using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Salt { get; set; } = "";
        public ICollection<UserRole> UserRoles { get; set; } = [];
        public ICollection<UserClaim> UserClaims { get; set; } = [];
    }
}
