using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class Claim
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}
