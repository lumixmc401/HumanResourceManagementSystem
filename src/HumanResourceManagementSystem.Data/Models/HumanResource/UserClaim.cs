using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class UserClaim
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
