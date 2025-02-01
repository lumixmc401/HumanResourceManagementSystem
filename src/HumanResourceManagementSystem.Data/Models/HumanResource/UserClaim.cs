using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class UserClaim
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string ClaimType { get; set; } = "";
        public string ClaimValue { get; set; } = "";
    }
}
